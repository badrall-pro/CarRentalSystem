using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Commands.UpdateVehicleType
{
    public record UpdateVehicleTypeCommand : IRequest<UpdateVehicleTypeResponse>
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int PassengerCapacity { get; init; }
        public decimal BaseDailyRate { get; init; }
    }

    public record UpdateVehicleTypeResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int PassengerCapacity { get; init; }
        public decimal BaseDailyRate { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public string Message { get; init; } = string.Empty;
    }
}
