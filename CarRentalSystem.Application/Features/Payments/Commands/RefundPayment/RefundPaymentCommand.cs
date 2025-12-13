using MediatR;

namespace CarRentalSystem.Application.Features.Payments.Commands.RefundPayment
{
    public record RefundPaymentCommand(Guid Id) : IRequest<RefundPaymentResponse>;

    public record RefundPaymentResponse
    {
        public Guid Id { get; init; }
        public decimal Amount { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public DateTime? UpdatedAt { get; init; }
    }
}
