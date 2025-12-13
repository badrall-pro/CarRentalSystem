using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Commands.UpdateVehicleType
{
    public class UpdateVehicleTypeCommandHandler : IRequestHandler<UpdateVehicleTypeCommand, UpdateVehicleTypeResponse>
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public UpdateVehicleTypeCommandHandler(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<UpdateVehicleTypeResponse> Handle(
            UpdateVehicleTypeCommand request,
            CancellationToken cancellationToken)
        {
            var vehicleType = await _vehicleTypeRepository.GetByIdAsync(request.Id, cancellationToken);
            if (vehicleType == null)
            {
                throw new KeyNotFoundException($"Vehicle type with ID '{request.Id}' not found.");
            }

            // Check if new name conflicts with another vehicle type
            var existingByName = await _vehicleTypeRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existingByName != null && existingByName.Id != request.Id)
            {
                throw new InvalidOperationException($"A vehicle type with name '{request.Name}' already exists.");
            }

            vehicleType.Update(
                name: request.Name,
                description: request.Description,
                passengerCapacity: request.PassengerCapacity,
                baseDailyRate: request.BaseDailyRate
            );

            await _vehicleTypeRepository.UpdateAsync(vehicleType, cancellationToken);

            return new UpdateVehicleTypeResponse
            {
                Id = vehicleType.Id,
                Name = vehicleType.Name,
                Description = vehicleType.Description,
                PassengerCapacity = vehicleType.PassengerCapacity,
                BaseDailyRate = vehicleType.BaseDailyRate,
                UpdatedAt = vehicleType.UpdatedAt,
                Message = "Vehicle type updated successfully"
            };
        }
    }
}
