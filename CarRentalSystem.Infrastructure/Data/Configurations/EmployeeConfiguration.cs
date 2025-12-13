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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.EmployeeNumber).IsRequired().HasMaxLength(50);
            builder.HasIndex(e => e.EmployeeNumber).IsUnique().HasDatabaseName("IX_Users_EmployeeNumber");

            builder.Property(e => e.Salary).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(e => e.HireDate).IsRequired();
        }
    }
}
