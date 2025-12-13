using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalSystem.Infrastructure.Data.Configurations
{
    public class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
    {
        public void Configure(EntityTypeBuilder<VehicleType> builder)
        {
            builder.ToTable("VehicleTypes");

            builder.HasKey(vt => vt.Id);

            builder.Property(vt => vt.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(vt => vt.Name)
                .IsUnique()
                .HasDatabaseName("IX_VehicleTypes_Name");

            builder.Property(vt => vt.Description)
                .HasMaxLength(500);

            builder.Property(vt => vt.PassengerCapacity)
                .IsRequired();

            builder.Property(vt => vt.BaseDailyRate)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(vt => vt.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(vt => vt.CreatedAt)
                .IsRequired();

            builder.Property(vt => vt.UpdatedAt)
                .IsRequired(false);

            // Navigation property is configured via VehicleConfiguration
        }
    }
}
