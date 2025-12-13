using MediatR;

namespace CarRentalSystem.Application.Features.Vehicles.Queries.GetVehicleById
{
    public record GetVehicleByIdQuery(Guid Id) : IRequest<GetVehicleByIdResponse>;

    public record GetVehicleByIdResponse
    {
        public Guid Id { get; init; }
        public string Brand { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public int Year { get; init; }
        public string LicensePlate { get; init; } = string.Empty;
        public string Color { get; init; } = string.Empty;
        public int Mileage { get; init; }
        public string Status { get; init; } = string.Empty;
        public decimal? DailyRate { get; init; }
        public decimal EffectiveDailyRate { get; init; }
        public int PassengerCapacity { get; init; }
        public string? ImageUrl { get; init; }
        public Guid VehicleTypeId { get; init; }
        public string VehicleTypeName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
