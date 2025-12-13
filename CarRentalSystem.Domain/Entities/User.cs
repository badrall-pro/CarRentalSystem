using CarRentalSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Domain.Entities
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string FirstName { get; protected set; } = string.Empty;
        public string LastName { get; protected set; } = string.Empty;
        public string Email { get; protected set; } = string.Empty;
        public string PasswordHash { get; protected set; } = string.Empty;
        public UserType UserType { get; protected set; }
        public bool IsActive { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        //COMPUTED PROPERTY
        public string FullName  => $"{FirstName} {LastName}";

        protected User() { }

        public static User CreateAdministrator(
        string firstName,
        string lastName,
        string email,
        string passwordHash)
        {
            ValidateCommonFields(firstName, lastName, email);

            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email.ToLower().Trim(),
                PasswordHash = passwordHash,
                UserType = UserType.Administrator,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        protected static void ValidateCommonFields(string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty", nameof(firstName));

            if (firstName.Length > 100)
                throw new ArgumentException("First name cannot exceed 100 characters", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty", nameof(lastName));

            if (lastName.Length > 100)
                throw new ArgumentException("Last name cannot exceed 100 characters", nameof(lastName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (!email.Contains('@') || !email.Contains('.'))
                throw new ArgumentException("Email must be valid", nameof(email));

            if (email.Length > 255)
                throw new ArgumentException("Email cannot exceed 255 characters", nameof(email));
        }

        public virtual void Update(string firstName, string lastName, string email)
        {
            ValidateCommonFields(firstName, lastName, email);

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UpdateTimestamp();
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash cannot be empty", nameof(newPasswordHash));

            PasswordHash = newPasswordHash;
            UpdateTimestamp();
        }
        public void Deactivate()
        {
            IsActive = false;
            UpdateTimestamp();
        }

        public void Activate()
        {
            IsActive = true;
            UpdateTimestamp();
        }

        public void ChangeRole(UserType newRole)
        {
            UserType = newRole;
            UpdateTimestamp();
        }

        protected void InitializeBaseProperties(
           Guid id,
           string firstName,
           string lastName,
           string email,
           string passwordHash,
           UserType userType,
           bool isActive,
           DateTime createdAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            UserType = userType;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        // TYPE CHECK
        public bool IsAdministrator() => UserType == UserType.Administrator;
        public bool IsEmployee() => UserType == UserType.Employee;
        public bool IsCustomer() => UserType == UserType.Customer;


        // SETTERS - ALLOWS CHILD CLASSES TO SET PROPERTIES
        protected void SetId(Guid id) => Id = id;
        protected void SetUserType(UserType userType) => UserType = userType;
        protected void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;

        protected void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
        
        //protected void SetFirstName(string name)
        //=> FirstName = string.IsNullOrWhiteSpace(name)
        //    ? throw new ArgumentException("First name cannot be empty")
        //    : name.Trim();

        //protected void SetLastName(string name)
        //    => LastName = string.IsNullOrWhiteSpace(name)
        //        ? throw new ArgumentException("Last name cannot be empty")
        //        : name.Trim();

        //protected void SetEmail(string email)
        //    => Email = string.IsNullOrWhiteSpace(email) || !email.Contains("@")
        //        ? throw new ArgumentException("Email is invalid")
        //        : email.Trim().ToLower();

        //protected void SetPasswordHash(string hash)
        //    => PasswordHash = string.IsNullOrWhiteSpace(hash)
        //        ? throw new ArgumentException("Password hash cannot be empty")
        //        : hash;
    }
}
