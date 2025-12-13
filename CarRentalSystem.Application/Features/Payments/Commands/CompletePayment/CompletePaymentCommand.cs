using MediatR;

namespace CarRentalSystem.Application.Features.Payments.Commands.CompletePayment
{
    public record CompletePaymentCommand(Guid Id) : IRequest<CompletePaymentResponse>;

    public record CompletePaymentResponse
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public DateTime? UpdatedAt { get; init; }
    }
}
