using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Commands.ConfirmReservation
{
    public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand, ConfirmReservationResponse>
    {
        private readonly IReservationRepository _reservationRepository;

        public ConfirmReservationCommandHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ConfirmReservationResponse> Handle(
            ConfirmReservationCommand request,
            CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID '{request.Id}' not found.");
            }

            reservation.Confirm();
            await _reservationRepository.UpdateAsync(reservation, cancellationToken);

            return new ConfirmReservationResponse
            {
                Id = reservation.Id,
                Status = reservation.Status.ToString(),
                Message = "Reservation confirmed successfully.",
                UpdatedAt = reservation.UpdatedAt
            };
        }
    }
}
