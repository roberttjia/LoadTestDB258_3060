using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LoadTest.Data.Configuration
{
    public static class DatabaseInitializer
    {
        public static async Task CreateLoginAndDatabaseAsync(IConfiguration configuration, string connectionStringName, ILogger logger)
        {
            logger.LogInformation("Creating login and database...");
            
            var connectionString = configuration.GetConnectionString(connectionStringName);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");
            }
            
            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;
            var userId = builder.UserID;
            var password = builder.Password;
            var serverName = builder.DataSource;
            
            // Connect to master database to create login and database
            builder.InitialCatalog = "master";
            var masterConnectionString = builder.ConnectionString;
            
            logger.LogInformation("Connecting to master database to create login and database...");
            
            using var connection = new SqlConnection(masterConnectionString);
            await connection.OpenAsync();
            
            // Create login if it doesn't exist
            logger.LogInformation($"Creating login '{userId}' if it doesn't exist...");
            var createLoginSql = $@"
                IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = '{userId}')
                BEGIN
                    CREATE LOGIN [{userId}] WITH PASSWORD = '{password}', CHECK_POLICY = OFF;
                END";
            
            using var loginCommand = new SqlCommand(createLoginSql, connection);
            await loginCommand.ExecuteNonQueryAsync();
            
            // Create database if it doesn't exist
            logger.LogInformation($"Creating database '{databaseName}' if it doesn't exist...");
            var createDbSql = $@"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}')
                BEGIN
                    CREATE DATABASE [{databaseName}];
                END";
            
            using var dbCommand = new SqlCommand(createDbSql, connection);
            await dbCommand.ExecuteNonQueryAsync();
            
            // Switch to the target database and create user
            builder.InitialCatalog = databaseName;
            using var dbConnection = new SqlConnection(builder.ConnectionString);
            await dbConnection.OpenAsync();
            
            logger.LogInformation($"Creating user '{userId}' in database '{databaseName}'...");
            var createUserSql = $@"
                IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '{userId}')
                BEGIN
                    CREATE USER [{userId}] FOR LOGIN [{userId}];
                    ALTER ROLE db_owner ADD MEMBER [{userId}];
                END";
            
            using var userCommand = new SqlCommand(createUserSql, dbConnection);
            await userCommand.ExecuteNonQueryAsync();
            
            logger.LogInformation("✅ Login, database, and user setup completed successfully!");
        }
        
        public static async Task<bool> CanConnectAsync(IConfiguration configuration, string connectionStringName, ILogger logger)
        {
            try
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString(connectionStringName)));
                
                var serviceProvider = serviceCollection.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                return await context.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                logger.LogWarning("Failed to connect to database using {ConnectionStringName}\n{Exception}", connectionStringName, ex);
                return false;
            }
        }
        
        public static async Task InitializeDatabaseAsync(IConfiguration configuration, string connectionStringName, ILogger logger)
        {
            logger.LogInformation("Initializing database with connection: {ConnectionStringName}", connectionStringName);
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(connectionStringName)));
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            logger.LogInformation("Creating database if it doesn't exist...");
            await context.Database.EnsureCreatedAsync();
            
            logger.LogInformation("Database initialization completed successfully.");
        }
    }
}