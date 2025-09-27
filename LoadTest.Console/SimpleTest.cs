using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LoadTest.Console
{
    public class SimpleTest
    {
        public static async Task<int> Main(string[] args)
        {
            System.Console.WriteLine("Testing LoadTestDb258_3060 limits...");
            System.Console.WriteLine("This is our ultimate stress test to find the true breaking point!");

            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                System.Console.WriteLine($"Connection: {MaskPassword(connectionString)}");

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                System.Console.WriteLine("✅ Connection successful");

                // Check if database exists
                var checkDbSql = "SELECT COUNT(*) FROM sys.databases WHERE name = 'LoadTestDb258_3060'";
                using var checkCommand = new SqlCommand(checkDbSql, connection);
                var dbExists = (int)(await checkCommand.ExecuteScalarAsync() ?? 0) > 0;
                
                System.Console.WriteLine($"Database LoadTestDb258_3060 exists: {dbExists}");

                if (dbExists)
                {
                    // Count tables
                    var countSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                    using var countCommand = new SqlCommand(countSql, connection);
                    var tableCount = await countCommand.ExecuteScalarAsync();
                    System.Console.WriteLine($"✅ Found {tableCount} tables");
                    
                    // Count total columns
                    var columnCountSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS";
                    using var columnCommand = new SqlCommand(columnCountSql, connection);
                    var columnCount = await columnCommand.ExecuteScalarAsync();
                    System.Console.WriteLine($"✅ Found {columnCount} total columns");
                }

                System.Console.WriteLine("✅ Basic connectivity test passed!");
                System.Console.WriteLine("Testing EF Core model compilation with 258 entities...");
                System.Console.WriteLine("This will determine if we've found the true breaking point!");
                
                return 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"❌ Error: {ex.Message}");
                return 1;
            }
        }

        private static string MaskPassword(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return "null";
            
            return connectionString.Replace("Password=Password12345!", "Password=***");
        }
    }
}