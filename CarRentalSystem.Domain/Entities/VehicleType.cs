using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Domain.Entities
{
    public class VehicleType
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int PassengerCapacity { get; private set; }
        public decimal BaseDailyRate { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Navigation property
        public ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();

        private VehicleType() { }

        public static VehicleType Create(
            string name,
            string description,
            int passengerCapacity,
            decimal baseDailyRate)
        {
                ValidateVTypeInfo(name, passengerCapacity, baseDailyRate);

                return new VehicleType
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Description = description?.Trim() ?? string.Empty,
                PassengerCapacity = passengerCapacity,
                BaseDailyRate = baseDailyRate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Update(string name, string description, int passengerCapacity, decimal baseDailyRate)
        {
            ValidateVTypeInfo(name, passengerCapacity, baseDailyRate);

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            PassengerCapacity = passengerCapacity;
            BaseDailyRate = baseDailyRate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public static void ValidateVTypeInfo(string name, int passengerCapacity, decimal baseDailyRate)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be empty", nameof(name));

                if (passengerCapacity <= 0)
                    throw new ArgumentException("Passenger capacity must be positive", nameof(passengerCapacity));

                if (baseDailyRate <= 0)
                    throw new ArgumentException("Daily rate must be positive", nameof(baseDailyRate));
            }
    }
}
