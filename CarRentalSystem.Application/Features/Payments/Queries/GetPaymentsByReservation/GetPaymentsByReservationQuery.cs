using MediatR;

namespace CarRentalSystem.Application.Features.Payments.Queries.GetPaymentsByReservation
{
    public record GetPaymentsByReservationQuery(Guid ReservationId) : IRequest<GetPaymentsByReservationResponse>;

    public record PaymentDto
    {
        public Guid Id { get; init; }
        public decimal Amount { get; init; }
        public string Method { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime PaymentDate { get; init; }
        public string? TransactionReference { get; init; }
        public string? Notes { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record GetPaymentsByReservationResponse
    {
        public Guid ReservationId { get; init; }
        public decimal TotalAmount { get; init; }
        public decimal TotalPaid { get; init; }
        public decimal RemainingBalance { get; init; }
        public IEnumerable<PaymentDto> Payments { get; init; } = Enumerable.Empty<PaymentDto>();
    }
}
