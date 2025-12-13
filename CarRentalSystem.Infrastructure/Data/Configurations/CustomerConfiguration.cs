using CarRentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(c => c.PhoneNumber).IsUnique().HasDatabaseName("IX_Users_PhoneNumber");

            builder.Property(c => c.Address).IsRequired().HasMaxLength(200);

            builder.Property(c => c.LicenseNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(c => c.LicenseNumber).IsUnique().HasDatabaseName("IX_Users_LicenseNumber");

            builder.Property(c => c.DateOfBirth)
            .IsRequired()
            .HasColumnType("date"); // STORE ONLY DATE, NOT TIME

            // IGNORE
            builder.Ignore(c => c.Age);
        }
    }
}
