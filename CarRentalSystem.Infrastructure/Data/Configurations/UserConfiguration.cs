using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.HasDiscriminator(u => u.UserType)
                .HasValue<User>(UserType.Administrator)
                .HasValue<Employee>(UserType.Employee)
                .HasValue<Customer>(UserType.Customer);

            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);

            builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);

            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IX_Users_Email");

            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);

            builder.Property(u => u.UserType).HasConversion<string>().IsRequired().HasMaxLength(50);

            builder.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
            .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .IsRequired(false);

            builder.Ignore(u => u.FullName);

        }
    }
}
