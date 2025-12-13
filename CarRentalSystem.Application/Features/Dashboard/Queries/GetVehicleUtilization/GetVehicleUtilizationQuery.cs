using MediatR;

namespace CarRentalSystem.Application.Features.Dashboard.Queries.GetVehicleUtilization
{
    public record GetVehicleUtilizationQuery : IRequest<GetVehicleUtilizationResponse>
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    public record VehicleUtilizationDto
    {
        public Guid VehicleId { get; init; }
        public string VehicleInfo { get; init; } = string.Empty;
        public string VehicleType { get; init; } = string.Empty;
        public int TotalRentalDays { get; init; }
        public int ReservationCount { get; init; }
        public decimal TotalRevenue { get; init; }
        public double UtilizationPercentage { get; init; }
    }

    public record GetVehicleUtilizationResponse
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public int TotalDays { get; init; }
        public double AverageUtilization { get; init; }
        public IEnumerable<VehicleUtilizationDto> VehicleUtilizations { get; init; } = Enumerable.Empty<VehicleUtilizationDto>();
    }
}
