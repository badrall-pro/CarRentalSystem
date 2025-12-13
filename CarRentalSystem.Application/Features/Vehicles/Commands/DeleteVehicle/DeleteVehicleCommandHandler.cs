using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Vehicles.Commands.DeleteVehicle
{
    public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, DeleteVehicleResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IReservationRepository _reservationRepository;

        public DeleteVehicleCommandHandler(
            IVehicleRepository vehicleRepository,
            IReservationRepository reservationRepository)
        {
            _vehicleRepository = vehicleRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<DeleteVehicleResponse> Handle(
            DeleteVehicleCommand request,
            CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID '{request.Id}' not found.");
            }

            // Check for active reservations
            var reservations = await _reservationRepository.GetByVehicleIdAsync(request.Id, cancellationToken);
            var hasActiveReservations = reservations.Any(r =>
                r.Status == ReservationStatus.Pending ||
                r.Status == ReservationStatus.Confirmed ||
                r.Status == ReservationStatus.Active);

            if (hasActiveReservations)
            {
                // Mark as out of service instead of deleting
                vehicle.SetStatus(VehicleStatus.OutOfService);
                await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
                return new DeleteVehicleResponse
                {
                    Success = true,
                    Message = "Vehicle has been marked as out of service because it has active reservations."
                };
            }

            await _vehicleRepository.DeleteAsync(request.Id, cancellationToken);

            return new DeleteVehicleResponse
            {
                Success = true,
                Message = "Vehicle deleted successfully."
            };
        }
    }
}
