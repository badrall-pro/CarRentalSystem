using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Queries.GetVehicleTypeById
{
    public record GetVehicleTypeByIdQuery(Guid Id) : IRequest<GetVehicleTypeByIdResponse>;

    public record GetVehicleTypeByIdResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int PassengerCapacity { get; init; }
        public decimal BaseDailyRate { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public int VehicleCount { get; init; }
    }
}
