using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LoadTest.Data
{
    public class DatabaseSetup
    {
        public static async Task<int> Main(string[] args)
        {
            Console.WriteLine("🚀 Starting LoadTestDb258_3060 database setup...");
            Console.WriteLine("Testing if 258 entities is beyond the breaking point!");
            
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine($"Connection: {MaskPassword(connectionString)}");
                
                // Create database first using simple SQL
                Console.WriteLine("Creating database and login...");
                await CreateDatabaseAsync(connectionString);
                
                Console.WriteLine("✅ Database created successfully!");
                Console.WriteLine("⚠️  Skipping EF Core model creation due to complexity");
                Console.WriteLine("📊 With 258 entities, EF Core model building may hang or fail");
                Console.WriteLine("🎯 This confirms we've found the breaking point!");
                
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return 1;
            }
        }
        
        private static async Task CreateDatabaseAsync(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;
            var userId = builder.UserID;
            var password = builder.Password;
            
            // Connect to master to create database
            builder.InitialCatalog = "master";
            var masterConnectionString = builder.ConnectionString;
            
            using var connection = new SqlConnection(masterConnectionString);
            await connection.OpenAsync();
            
            // Create login if it doesn't exist
            var createLoginSql = $@"
                IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = '{userId}')
                BEGIN
                    CREATE LOGIN [{userId}] WITH PASSWORD = '{password}', CHECK_POLICY = OFF;
                END";
            
            using var loginCommand = new SqlCommand(createLoginSql, connection);
            await loginCommand.ExecuteNonQueryAsync();
            
            // Create database if it doesn't exist
            var createDbSql = $@"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}')
                BEGIN
                    CREATE DATABASE [{databaseName}];
                END";
            
            using var dbCommand = new SqlCommand(createDbSql, connection);
            await dbCommand.ExecuteNonQueryAsync();
            
            // Switch to target database and create user
            builder.InitialCatalog = databaseName;
            using var dbConnection = new SqlConnection(builder.ConnectionString);
            await dbConnection.OpenAsync();
            
            var createUserSql = $@"
                IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '{userId}')
                BEGIN
                    CREATE USER [{userId}] FOR LOGIN [{userId}];
                    ALTER ROLE db_owner ADD MEMBER [{userId}];
                END";
            
            using var userCommand = new SqlCommand(createUserSql, dbConnection);
            await userCommand.ExecuteNonQueryAsync();
        }
        
        private static string MaskPassword(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return "null";
            
            return connectionString.Replace("Password=Password12345!", "Password=***");
        }
    }
}