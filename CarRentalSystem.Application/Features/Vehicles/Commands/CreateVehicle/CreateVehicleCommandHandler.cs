using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Vehicles.Commands.CreateVehicle
{
    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, CreateVehicleResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public CreateVehicleCommandHandler(
            IVehicleRepository vehicleRepository,
            IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<CreateVehicleResponse> Handle(
            CreateVehicleCommand request,
            CancellationToken cancellationToken)
        {
            // Validate vehicle type exists
            var vehicleType = await _vehicleTypeRepository.GetByIdAsync(request.VehicleTypeId, cancellationToken);
            if (vehicleType == null)
            {
                throw new KeyNotFoundException($"Vehicle type with ID '{request.VehicleTypeId}' not found.");
            }

            // Check if license plate already exists
            var licensePlateExists = await _vehicleRepository.LicensePlateExistsAsync(request.LicensePlate, cancellationToken);
            if (licensePlateExists)
            {
                throw new InvalidOperationException($"A vehicle with license plate '{request.LicensePlate}' already exists.");
            }

            // Create the vehicle entity
            var vehicle = Vehicle.Create(
                brand: request.Brand,
                model: request.Model,
                year: request.Year,
                licensePlate: request.LicensePlate,
                color: request.Color,
                mileage: request.Mileage,
                vehicleTypeId: request.VehicleTypeId,
                dailyRate: request.DailyRate,
                imageUrl: request.ImageUrl
            );

            await _vehicleRepository.AddAsync(vehicle, cancellationToken);

            // Fetch with VehicleType loaded so GetEffectiveDailyRate() works
            var savedVehicle = await _vehicleRepository.GetByIdWithTypeAsync(vehicle.Id, cancellationToken);

            return new CreateVehicleResponse
            {
                Id = savedVehicle!.Id,
                Brand = savedVehicle.Brand,
                Model = savedVehicle.Model,
                Year = savedVehicle.Year,
                LicensePlate = savedVehicle.LicensePlate,
                Color = savedVehicle.Color,
                Status = savedVehicle.Status.ToString(),
                EffectiveDailyRate = savedVehicle.GetEffectiveDailyRate(),
                CreatedAt = savedVehicle.CreatedAt
            };
        }
    }
}
