using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Reservations.Commands.StartReservation
{
    public class StartReservationCommandHandler : IRequestHandler<StartReservationCommand, StartReservationResponse>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public StartReservationCommandHandler(
            IReservationRepository reservationRepository,
            IVehicleRepository vehicleRepository)
        {
            _reservationRepository = reservationRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<StartReservationResponse> Handle(
            StartReservationCommand request,
            CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID '{request.Id}' not found.");
            }

            reservation.StartRental();
            await _reservationRepository.UpdateAsync(reservation, cancellationToken);

            // Mark vehicle as rented
            var vehicle = reservation.Vehicle;
            vehicle.MarkAsRented();
            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

            return new StartReservationResponse
            {
                Id = reservation.Id,
                Status = reservation.Status.ToString(),
                Message = "Rental started successfully. Vehicle is now marked as rented.",
                UpdatedAt = reservation.UpdatedAt
            };
        }
    }
}
