using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Commands.CreateVehicleType
{
    public class CreateVehicleTypeCommandHandler : IRequestHandler<CreateVehicleTypeCommand, CreateVehicleTypeResponse>
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public CreateVehicleTypeCommandHandler(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<CreateVehicleTypeResponse> Handle(
            CreateVehicleTypeCommand request,
            CancellationToken cancellationToken)
        {
            // Check if name already exists
            var nameExists = await _vehicleTypeRepository.NameExistsAsync(request.Name, cancellationToken);
            if (nameExists)
            {
                throw new InvalidOperationException($"A vehicle type with name '{request.Name}' already exists.");
            }

            // Create the vehicle type entity
            var vehicleType = VehicleType.Create(
                name: request.Name,
                description: request.Description,
                passengerCapacity: request.PassengerCapacity,
                baseDailyRate: request.BaseDailyRate
            );

            // Save to database
            await _vehicleTypeRepository.AddAsync(vehicleType, cancellationToken);

            return new CreateVehicleTypeResponse
            {
                Id = vehicleType.Id,
                Name = vehicleType.Name,
                Description = vehicleType.Description,
                PassengerCapacity = vehicleType.PassengerCapacity,
                BaseDailyRate = vehicleType.BaseDailyRate,
                CreatedAt = vehicleType.CreatedAt
            };
        }
    }
}
