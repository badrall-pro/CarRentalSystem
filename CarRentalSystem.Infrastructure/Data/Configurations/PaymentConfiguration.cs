using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalSystem.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Method)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(p => p.TransactionReference)
                .HasMaxLength(200);

            builder.Property(p => p.Notes)
                .HasMaxLength(500);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .IsRequired(false);

            // Foreign key relationship with Reservation
            builder.HasOne(p => p.Reservation)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for common queries
            builder.HasIndex(p => p.ReservationId)
                .HasDatabaseName("IX_Payments_ReservationId");

            builder.HasIndex(p => p.Status)
                .HasDatabaseName("IX_Payments_Status");

            builder.HasIndex(p => p.PaymentDate)
                .HasDatabaseName("IX_Payments_PaymentDate");
        }
    }
}
