using MediatR;

namespace CarRentalSystem.Application.Features.Vehicles.Commands.UpdateVehicle
{
    public record UpdateVehicleCommand : IRequest<UpdateVehicleResponse>
    {
        public Guid Id { get; init; }
        public string Brand { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public int Year { get; init; }
        public string Color { get; init; } = string.Empty;
        public int Mileage { get; init; }
        public Guid VehicleTypeId { get; init; }
        public decimal? DailyRate { get; init; }
        public string? ImageUrl { get; init; }
    }

    public record UpdateVehicleResponse
    {
        public Guid Id { get; init; }
        public string Brand { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public int Year { get; init; }
        public string LicensePlate { get; init; } = string.Empty;
        public string Color { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public decimal EffectiveDailyRate { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
