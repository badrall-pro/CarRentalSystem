using CarRentalSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public ReservationStatus Status { get; private set; }
        public string? Notes { get; private set; }
        public string? QRCode { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Foreign keys
        public Guid CustomerId { get; private set; }
        public Guid VehicleId { get; private set; }

        // Navigation properties
        public Customer Customer { get; private set; } = null!;
        public Vehicle Vehicle { get; private set; } = null!;
        public ICollection<Payment> Payments { get; private set; } = new List<Payment>();

        private Reservation() { }

        public static Reservation Create(
            Guid customerId,
            Guid vehicleId,
            DateTime startDate,
            DateTime endDate,
            decimal dailyRate,
            string? notes = null)
        {
            ValidateDates(startDate, endDate);

            var days = (int)Math.Ceiling((endDate - startDate).TotalDays);
            var totalAmount = days * dailyRate;

            return new Reservation
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                VehicleId = vehicleId,
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                TotalAmount = totalAmount,
                Status = ReservationStatus.Pending,
                Notes = notes,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Confirm()
        {
            if (Status != ReservationStatus.Pending)
                throw new InvalidOperationException("Only pending reservations can be confirmed");

            Status = ReservationStatus.Confirmed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void StartRental()
        {
            if (Status != ReservationStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed reservations can be started");

            Status = ReservationStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (Status != ReservationStatus.Active)
                throw new InvalidOperationException("Only active reservations can be completed");

            Status = ReservationStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == ReservationStatus.Completed)
                throw new InvalidOperationException("Cannot cancel a completed reservation");

            Status = ReservationStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetQRCode(string qrCode)
        {
            QRCode = qrCode;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDates(DateTime startDate, DateTime endDate, decimal dailyRate)
        {
            if (Status != ReservationStatus.Pending)
                throw new InvalidOperationException("Can only update dates for pending reservations");

            ValidateDates(startDate, endDate);

            StartDate = startDate.Date;
            EndDate = endDate.Date;

            var days = (int)Math.Ceiling((endDate - startDate).TotalDays);
            TotalAmount = days * dailyRate;
            UpdatedAt = DateTime.UtcNow;
        }

        public int GetRentalDays() => (int)Math.Ceiling((EndDate - StartDate).TotalDays);

        public decimal GetRemainingBalance()
        {
            var totalPaid = Payments
                .Where(p => p.Status == PaymentStatus.Completed)
                .Sum(p => p.Amount);

            return TotalAmount - totalPaid;
        }

        public decimal GetRemainingBalance(decimal amountPaid)
        {
            return TotalAmount - amountPaid;
        }

        public bool IsFullyPaid() => GetRemainingBalance() <= 0;

        private static void ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Start date cannot be in the past", nameof(startDate));

            if (endDate.Date <= startDate.Date)
                throw new ArgumentException("End date must be after start date", nameof(endDate));

            var maxRentalDays = 90;
            var days = (endDate - startDate).TotalDays;
            if (days > maxRentalDays)
                throw new ArgumentException($"Rental period cannot exceed {maxRentalDays} days", nameof(endDate));
        }
    }
}
