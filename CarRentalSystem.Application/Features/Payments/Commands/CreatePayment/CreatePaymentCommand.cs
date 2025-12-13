using CarRentalSystem.Domain.Enums;
using MediatR;

namespace CarRentalSystem.Application.Features.Payments.Commands.CreatePayment
{
    public record CreatePaymentCommand : IRequest<CreatePaymentResponse>
    {
        public Guid ReservationId { get; init; }
        public decimal Amount { get; init; }
        public PaymentMethod Method { get; init; }
        public string? TransactionReference { get; init; }
        public string? Notes { get; init; }
    }

    public record CreatePaymentResponse
    {
        public Guid Id { get; init; }
        public Guid ReservationId { get; init; }
        public decimal Amount { get; init; }
        public string Method { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime PaymentDate { get; init; }
        public decimal RemainingBalance { get; init; }
    }
}
