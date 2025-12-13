using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalSystem.Domain.Enums;

namespace CarRentalSystem.Domain.Entities
{
    public class Customer : User
    {
        public string PhoneNumber { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string LicenseNumber { get; private set; } = string.Empty;
        public DateTime DateOfBirth { get; private set; }
        
        // COMPUTED PROPERTY FOR AGE
        public int Age
        {
            get
            {
                var age = DateTime.UtcNow.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > DateTime.UtcNow.AddYears(-age))
                    age--;
                return age;
            }
        }

        private Customer() { }

        public static Customer Create(
           string firstName,
           string lastName,
           string email,
           string passwordHash,
           string phoneNumber,
           string address,
           string licenseNumber,
           DateTime dateOfBirth)
        {
            ValidateCommonFields(firstName, lastName, email);
            ValidateCustomerInfo(phoneNumber, address, licenseNumber);


            var customer = new Customer();

            // INITIALIZE BASE PROPERTIES
            customer.InitializeBaseProperties(
                Guid.NewGuid(),
                firstName,
                lastName,
                email.ToLower().Trim(),
                passwordHash,
                UserType.Customer,
                true,
                DateTime.UtcNow
            );

            // SET CUSTOMER SPECIFIC PROPERTIES
            customer.LicenseNumber = licenseNumber.Trim().ToUpper();
            customer.PhoneNumber = phoneNumber.Trim();
            customer.Address = address.Trim();
            customer.DateOfBirth = dateOfBirth.Date;

            return customer;
        }
        
        public void UpdateContactInfo(string phoneNumber, string address, string licenseNumber)
        {
            ValidateCustomerInfo(phoneNumber, address, licenseNumber);

            PhoneNumber = phoneNumber.Trim();
            Address = address.Trim();
            LicenseNumber = licenseNumber.Trim().ToUpper();
            UpdateTimestamp();
        }

        public bool IsEligible()
        {
            return Age >= 18;
        }

        //------------------ VALIDATIONS ------------------
        public static void ValidateCustomerInfo(string phoneNumber, string address, string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));
            if (phoneNumber.Length > 20)
                throw new ArgumentException("Phone number cannot exceed 20 characters", nameof(phoneNumber));

            if (string.IsNullOrWhiteSpace(licenseNumber))
                throw new ArgumentException("Driver license number cannot be empty", nameof(licenseNumber));
            if (licenseNumber.Length > 50)
                throw new ArgumentException("Driver license number cannot exceed 50 characters", nameof(licenseNumber));

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address cannot be empty", nameof(address));

        }

        public static void ValidateAge(DateTime dateOfBirth)
        {
            var age = DateTime.UtcNow.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > DateTime.UtcNow.AddYears(-age))
                age--;
            if (age < 18)
                throw new ArgumentException("Customer must be at least 18 years old", nameof(dateOfBirth));
            if (age > 120)
                throw new ArgumentException("Date of birth seems invalid", nameof(dateOfBirth));
            if (dateOfBirth > DateTime.UtcNow)
                throw new ArgumentException("Date of birth cannot be in the future", nameof(dateOfBirth));
        }
    }

}
