using MediatR;

namespace CarRentalSystem.Application.Features.Payments.Queries.GetPaymentById
{
    public record GetPaymentByIdQuery(Guid Id) : IRequest<GetPaymentByIdResponse>;

    public record GetPaymentByIdResponse
    {
        public Guid Id { get; init; }
        public Guid ReservationId { get; init; }
        public decimal Amount { get; init; }
        public string Method { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime PaymentDate { get; init; }
        public string? TransactionReference { get; init; }
        public string? Notes { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
