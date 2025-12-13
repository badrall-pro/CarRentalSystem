using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalSystem.Infrastructure.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Brand)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Year)
                .IsRequired();

            builder.Property(v => v.LicensePlate)
                .IsRequired()
                .HasMaxLength(20);
            builder.HasIndex(v => v.LicensePlate)
                .IsUnique()
                .HasDatabaseName("IX_Vehicles_LicensePlate");

            builder.Property(v => v.Color)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.Mileage)
                .IsRequired();

            builder.Property(v => v.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.ImageUrl)
                .HasMaxLength(500);

            builder.Property(v => v.DailyRate)
                .IsRequired(false)
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.CreatedAt)
                .IsRequired();

            builder.Property(v => v.UpdatedAt)
                .IsRequired(false);

            // Foreign key relationship with VehicleType
            builder.HasOne(v => v.VehicleType)
                .WithMany(vt => vt.Vehicles)
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(v => v.VehicleTypeId)
                .HasDatabaseName("IX_Vehicles_VehicleTypeId");

            builder.HasIndex(v => v.Status)
                .HasDatabaseName("IX_Vehicles_Status");
        }
    }
}
