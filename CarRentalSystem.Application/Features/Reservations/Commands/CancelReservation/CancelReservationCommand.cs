using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Commands.CancelReservation
{
    public record CancelReservationCommand(Guid Id) : IRequest<CancelReservationResponse>;

    public record CancelReservationResponse
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public DateTime? UpdatedAt { get; init; }
    }
}
