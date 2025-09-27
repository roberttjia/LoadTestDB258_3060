using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoadTest.Data
{
    public class TestEFCore
    {
        public static async Task<int> Main(string[] args)
        {
            Console.WriteLine("🧪 Testing EF Core with 258 entities...");
            Console.WriteLine("Let's see if EF Core can actually handle this!");
            
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine($"Connection: {MaskPassword(connectionString)}");
                
                Console.WriteLine("Creating DbContext...");
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;
                
                Console.WriteLine("Initializing ApplicationDbContext...");
                using var context = new ApplicationDbContext(options);
                
                Console.WriteLine("Testing database connection...");
                var canConnect = await context.Database.CanConnectAsync();
                Console.WriteLine($"Can connect: {canConnect}");
                
                if (canConnect)
                {
                    Console.WriteLine("Creating database schema...");
                    await context.Database.EnsureCreatedAsync();
                    Console.WriteLine("✅ Database schema created successfully!");
                    
                    // Count the actual tables created
                    var tableCountSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                    var tableCount = await context.Database.ExecuteSqlRawAsync(tableCountSql);
                    Console.WriteLine($"✅ EF Core successfully handled 258 entities!");
                }
                
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
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