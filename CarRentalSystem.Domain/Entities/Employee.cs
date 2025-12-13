using CarRentalSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Domain.Entities
{
    public class Employee : User
    {
        public string EmployeeNumber { get; private set; } = string.Empty;
        public decimal Salary { get; private set; }
        public DateTime HireDate { get; private set; }

        private Employee() { }

        public static Employee Create(
            string firstName,
            string lastName,
            string email,
            string passwordHash,
            string employeeNumber,
            decimal salary,
            DateTime? hireDate = null)
        {
            ValidateCommonFields(firstName, lastName, email);
            ValidateEmployeeInfo(employeeNumber, salary);

            var employee = new Employee();
            employee.InitializeBaseProperties(
                Guid.NewGuid(),
                firstName,
                lastName,
                email.ToLower().Trim(),
                passwordHash,
                UserType.Employee,
                true,
                DateTime.UtcNow
                );
            employee.EmployeeNumber = employeeNumber.Trim();
            employee.Salary = salary;
            employee.HireDate = hireDate ?? DateTime.UtcNow;

            return employee;
            
        }

        // Add Employee-specific methods here

        public void UpdateEmployeeInfo(string employeeNumber, decimal salary)
        {
            ValidateEmployeeInfo(employeeNumber, salary);

            EmployeeNumber = employeeNumber.Trim();
            Salary = salary;
            UpdateTimestamp();
        }

        //------------------ VALIDATIONS ------------------
        public static void ValidateEmployeeInfo(string employeeNumber,decimal salary)
        {
            if (string.IsNullOrWhiteSpace(employeeNumber))
                throw new ArgumentException("Employee number cannot be empty", nameof(employeeNumber));
            if (employeeNumber.Length > 50)
                throw new ArgumentException("Employee number cannot exceed 50 characters", nameof(employeeNumber));

            if (salary <= 0)
                throw new ArgumentException("Salary must be greater than zero", nameof(salary));
            if (salary > 1000000)
                throw new ArgumentException("Salary seems unreasonably high. Please verify.", nameof(salary));

        }
    }
}
