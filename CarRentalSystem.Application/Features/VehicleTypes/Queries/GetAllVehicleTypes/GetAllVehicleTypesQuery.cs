using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Queries.GetAllVehicleTypes
{
    public record GetAllVehicleTypesQuery : IRequest<GetAllVehicleTypesResponse>
    {
        public bool IncludeInactive { get; init; } = false;
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public record VehicleTypeDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int PassengerCapacity { get; init; }
        public decimal BaseDailyRate { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record GetAllVehicleTypesResponse
    {
        public IEnumerable<VehicleTypeDto> VehicleTypes { get; init; } = Enumerable.Empty<VehicleTypeDto>();
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
    }
}
