using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Vehicles.Queries.GetVehicleById
{
    public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, GetVehicleByIdResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetVehicleByIdQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<GetVehicleByIdResponse> Handle(
            GetVehicleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdWithTypeAsync(request.Id, cancellationToken);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID '{request.Id}' not found.");
            }

            return new GetVehicleByIdResponse
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year,
                LicensePlate = vehicle.LicensePlate,
                Color = vehicle.Color,
                Mileage = vehicle.Mileage,
                Status = vehicle.Status.ToString(),
                DailyRate = vehicle.DailyRate,
                EffectiveDailyRate = vehicle.GetEffectiveDailyRate(),
                PassengerCapacity = vehicle.VehicleType?.PassengerCapacity ?? 0,
                ImageUrl = vehicle.ImageUrl,
                VehicleTypeId = vehicle.VehicleTypeId,
                VehicleTypeName = vehicle.VehicleType?.Name ?? string.Empty,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt
            };
        }
    }
}
