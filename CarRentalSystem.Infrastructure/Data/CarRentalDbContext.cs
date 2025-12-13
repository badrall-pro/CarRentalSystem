using Microsoft.EntityFrameworkCore;
using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Infrastructure.Data
{
	public class CarRentalDbContext:DbContext
	{
        // THE GENERIC <CARRENTALDBCONTEXT> STRONGLY-TYPES THE OPTIONS, ENSURING THEY ARE USED ONLY FOR THIS CONTEXT(SESSION DB-APP).
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
			: base(options)
		{
        }
        // DbSets as we create entities
        public DbSet<User> Users => Set<User>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Customer> Customers => Set<Customer>();
        // Vehicles
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
        // Reservations
        public DbSet<Reservation> Reservations => Set<Reservation>();
        // Payments
        public DbSet<Payment> Payments => Set<Payment>();

        // THIS LIFECYCLE METHOD IS CALLED ONCE TO BUILD THE DATABASE MODEL (SCHEMA MAPPING)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            // AUTOLOADS ALL EXTERNAL ENTITY MAPPING CONFIGURATIONS FROM THIS ASSEMBLY (.dll).
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarRentalDbContext).Assembly);
        }
    }
}
