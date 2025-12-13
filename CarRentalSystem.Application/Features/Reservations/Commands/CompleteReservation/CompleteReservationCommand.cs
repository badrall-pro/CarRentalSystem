using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Commands.CompleteReservation
{
    public record CompleteReservationCommand : IRequest<CompleteReservationResponse>
    {
        public Guid Id { get; init; }
        public int? FinalMileage { get; init; }
    }

    public record CompleteReservationResponse
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
        public decimal TotalAmount { get; init; }
        public decimal AmountPaid { get; init; }
        public decimal RemainingBalance { get; init; }
        public string Message { get; init; } = string.Empty;
        public DateTime? UpdatedAt { get; init; }
    }
}
