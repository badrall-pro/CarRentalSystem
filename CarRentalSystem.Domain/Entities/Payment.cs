using CarRentalSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public string? TransactionReference { get; private set; }
        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Foreign key
        public Guid ReservationId { get; private set; }

        // Navigation property
        public Reservation Reservation { get; private set; } = null!;

        private Payment() { }

        public static Payment Create(
            Guid reservationId,
            decimal amount,
            PaymentMethod method,
            string? transactionReference = null,
            string? notes = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Payment amount must be positive", nameof(amount));

            return new Payment
            {
                Id = Guid.NewGuid(),
                ReservationId = reservationId,
                Amount = amount,
                Method = method,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow,
                TransactionReference = transactionReference,
                Notes = notes,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void MarkAsCompleted()
        {
            if (Status == PaymentStatus.Completed)
                throw new InvalidOperationException("Payment is already completed");

            Status = PaymentStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsFailed()
        {
            if (Status == PaymentStatus.Completed)
                throw new InvalidOperationException("Cannot mark completed payment as failed");

            Status = PaymentStatus.Failed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Refund()
        {
            if (Status != PaymentStatus.Completed)
                throw new InvalidOperationException("Can only refund completed payments");

            Status = PaymentStatus.Refunded;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
