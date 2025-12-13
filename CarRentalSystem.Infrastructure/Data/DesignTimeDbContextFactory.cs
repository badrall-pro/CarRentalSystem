using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CarRentalSystem.Infrastructure.Data
{
    /// <summary>
    /// Factory for creating DbContext at design-time for EF Core migrations.
    /// This bypasses the DI container which requires JWT_SECRET and other runtime config.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CarRentalDbContext>
    {
        public CarRentalDbContext CreateDbContext(string[] args)
        {
            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CarRentalSystem.API"))
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CarRentalDbContext>();
            
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            optionsBuilder.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly(typeof(CarRentalDbContext).Assembly.FullName));

            return new CarRentalDbContext(optionsBuilder.Options);
        }
    }
}
