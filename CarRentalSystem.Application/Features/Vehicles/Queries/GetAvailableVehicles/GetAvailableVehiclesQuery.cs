using MediatR;

namespace CarRentalSystem.Application.Features.Vehicles.Queries.GetAvailableVehicles
{
    public record GetAvailableVehiclesQuery : IRequest<GetAvailableVehiclesResponse>
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public Guid? VehicleTypeId { get; init; }
    }

    public record AvailableVehicleDto
    {
        public Guid Id { get; init; }
        public string Brand { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public int Year { get; init; }
        public string LicensePlate { get; init; } = string.Empty;
        public string Color { get; init; } = string.Empty;
        public decimal EffectiveDailyRate { get; init; }
        public string? ImageUrl { get; init; }
        public string VehicleTypeName { get; init; } = string.Empty;
        public int PassengerCapacity { get; init; }
    }

    public record GetAvailableVehiclesResponse
    {
        public IEnumerable<AvailableVehicleDto> Vehicles { get; init; } = Enumerable.Empty<AvailableVehicleDto>();
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
