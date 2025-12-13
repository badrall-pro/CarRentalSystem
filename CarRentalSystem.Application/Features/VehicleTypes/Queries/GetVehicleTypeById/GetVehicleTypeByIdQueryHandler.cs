using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Queries.GetVehicleTypeById
{
    public class GetVehicleTypeByIdQueryHandler : IRequestHandler<GetVehicleTypeByIdQuery, GetVehicleTypeByIdResponse>
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public GetVehicleTypeByIdQueryHandler(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<GetVehicleTypeByIdResponse> Handle(
            GetVehicleTypeByIdQuery request,
            CancellationToken cancellationToken)
        {
            var vehicleType = await _vehicleTypeRepository.GetByIdWithVehiclesAsync(request.Id, cancellationToken);
            if (vehicleType == null)
            {
                throw new KeyNotFoundException($"Vehicle type with ID '{request.Id}' not found.");
            }

            return new GetVehicleTypeByIdResponse
            {
                Id = vehicleType.Id,
                Name = vehicleType.Name,
                Description = vehicleType.Description,
                PassengerCapacity = vehicleType.PassengerCapacity,
                BaseDailyRate = vehicleType.BaseDailyRate,
                IsActive = vehicleType.IsActive,
                CreatedAt = vehicleType.CreatedAt,
                UpdatedAt = vehicleType.UpdatedAt,
                VehicleCount = vehicleType.Vehicles.Count
            };
        }
    }
}
