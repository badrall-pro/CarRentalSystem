using MediatR;

namespace CarRentalSystem.Application.Features.Vehicles.Commands.DeleteVehicle
{
    public record DeleteVehicleCommand(Guid Id) : IRequest<DeleteVehicleResponse>;

    public record DeleteVehicleResponse
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
    }
}
