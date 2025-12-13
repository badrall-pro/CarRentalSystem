using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Commands.CancelReservation
{
    public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, CancelReservationResponse>
    {
        private readonly IReservationRepository _reservationRepository;

        public CancelReservationCommandHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<CancelReservationResponse> Handle(
            CancelReservationCommand request,
            CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID '{request.Id}' not found.");
            }

            reservation.Cancel();
            await _reservationRepository.UpdateAsync(reservation, cancellationToken);

            return new CancelReservationResponse
            {
                Id = reservation.Id,
                Status = reservation.Status.ToString(),
                Message = "Reservation cancelled successfully.",
                UpdatedAt = reservation.UpdatedAt
            };
        }
    }
}
