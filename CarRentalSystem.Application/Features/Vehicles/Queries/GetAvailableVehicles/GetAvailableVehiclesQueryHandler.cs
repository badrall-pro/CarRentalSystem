using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.Vehicles.Queries.GetAvailableVehicles
{
    public class GetAvailableVehiclesQueryHandler : IRequestHandler<GetAvailableVehiclesQuery, GetAvailableVehiclesResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetAvailableVehiclesQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<GetAvailableVehiclesResponse> Handle(
            GetAvailableVehiclesQuery request,
            CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.GetAvailableVehiclesAsync(
                request.StartDate,
                request.EndDate,
                cancellationToken);

            var vehicleList = vehicles.ToList();

            // Apply vehicle type filter if specified
            if (request.VehicleTypeId.HasValue)
            {
                vehicleList = vehicleList.Where(v => v.VehicleTypeId == request.VehicleTypeId.Value).ToList();
            }

            var totalCount = vehicleList.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var pagedVehicles = vehicleList
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(v => new AvailableVehicleDto
                {
                    Id = v.Id,
                    Brand = v.Brand,
                    Model = v.Model,
                    Year = v.Year,
                    LicensePlate = v.LicensePlate,
                    Color = v.Color,
                    EffectiveDailyRate = v.GetEffectiveDailyRate(),
                    ImageUrl = v.ImageUrl,
                    VehicleTypeName = v.VehicleType?.Name ?? string.Empty,
                    PassengerCapacity = v.VehicleType?.PassengerCapacity ?? 0
                });

            return new GetAvailableVehiclesResponse
            {
                Vehicles = pagedVehicles,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
        }
    }
}
