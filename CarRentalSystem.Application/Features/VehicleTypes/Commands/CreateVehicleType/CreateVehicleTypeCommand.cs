using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Commands.CreateVehicleType
{
    public record CreateVehicleTypeCommand : IRequest<CreateVehicleTypeResponse>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int PassengerCapacity { get; init; }
        public decimal BaseDailyRate { get; init; }
    }

    public record CreateVehicleTypeResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int PassengerCapacity { get; init; }
        public decimal BaseDailyRate { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
