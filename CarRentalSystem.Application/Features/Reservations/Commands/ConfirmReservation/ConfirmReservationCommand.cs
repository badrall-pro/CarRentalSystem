using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Commands.ConfirmReservation
{
    public record ConfirmReservationCommand(Guid Id) : IRequest<ConfirmReservationResponse>;

    public record ConfirmReservationResponse
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public DateTime? UpdatedAt { get; init; }
    }
}
