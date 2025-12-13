using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Commands.DeleteVehicleType
{
    public record DeleteVehicleTypeCommand(Guid Id) : IRequest<DeleteVehicleTypeResponse>;

    public record DeleteVehicleTypeResponse
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
    }
}
