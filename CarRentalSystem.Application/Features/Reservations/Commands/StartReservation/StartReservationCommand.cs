using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Commands.StartReservation
{
    public record StartReservationCommand(Guid Id) : IRequest<StartReservationResponse>;

    public record StartReservationResponse
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public DateTime? UpdatedAt { get; init; }
    }
}
