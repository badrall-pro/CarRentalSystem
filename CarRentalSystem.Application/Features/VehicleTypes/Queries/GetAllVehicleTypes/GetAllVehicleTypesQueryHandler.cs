using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Queries.GetAllVehicleTypes
{
    public class GetAllVehicleTypesQueryHandler : IRequestHandler<GetAllVehicleTypesQuery, GetAllVehicleTypesResponse>
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public GetAllVehicleTypesQueryHandler(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<GetAllVehicleTypesResponse> Handle(
            GetAllVehicleTypesQuery request,
            CancellationToken cancellationToken)
        {
            var vehicleTypes = await _vehicleTypeRepository.GetAllAsync(request.IncludeInactive, cancellationToken);
            var vehicleTypeList = vehicleTypes.ToList();

            var totalCount = vehicleTypeList.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var pagedVehicleTypes = vehicleTypeList
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(vt => new VehicleTypeDto
                {
                    Id = vt.Id,
                    Name = vt.Name,
                    Description = vt.Description,
                    PassengerCapacity = vt.PassengerCapacity,
                    BaseDailyRate = vt.BaseDailyRate,
                    IsActive = vt.IsActive,
                    CreatedAt = vt.CreatedAt
                });

            return new GetAllVehicleTypesResponse
            { 
                VehicleTypes = pagedVehicleTypes,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };
        }
    }
}
