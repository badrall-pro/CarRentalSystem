using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Vehicles.Commands.UpdateVehicle
{
    public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, UpdateVehicleResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public UpdateVehicleCommandHandler(
            IVehicleRepository vehicleRepository,
            IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<UpdateVehicleResponse> Handle(
            UpdateVehicleCommand request,
            CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdWithTypeAsync(request.Id, cancellationToken);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID '{request.Id}' not found.");
            }

            // Validate vehicle type exists
            var vehicleType = await _vehicleTypeRepository.GetByIdAsync(request.VehicleTypeId, cancellationToken);
            if (vehicleType == null)
            {
                throw new KeyNotFoundException($"Vehicle type with ID '{request.VehicleTypeId}' not found.");
            }

            vehicle.Update(
                brand: request.Brand,
                model: request.Model,
                year: request.Year,
                color: request.Color,
                mileage: request.Mileage,
                vehicleTypeId: request.VehicleTypeId,
                dailyRate: request.DailyRate,
                imageUrl: request.ImageUrl
            );

            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

            // Reload with VehicleType to ensure GetEffectiveDailyRate() works after type change
            var updatedVehicle = await _vehicleRepository.GetByIdWithTypeAsync(vehicle.Id, cancellationToken);

            return new UpdateVehicleResponse
            {
                Id = updatedVehicle!.Id,
                Brand = updatedVehicle.Brand,
                Model = updatedVehicle.Model,
                Year = updatedVehicle.Year,
                LicensePlate = updatedVehicle.LicensePlate,
                Color = updatedVehicle.Color,
                Status = updatedVehicle.Status.ToString(),
                EffectiveDailyRate = updatedVehicle.GetEffectiveDailyRate(),
                UpdatedAt = updatedVehicle.UpdatedAt
            };
        }
    }
}
