using CarRentalSystem.Domain.Interfaces;
using MediatR;

namespace CarRentalSystem.Application.Features.VehicleTypes.Commands.DeleteVehicleType
{
    public class DeleteVehicleTypeCommandHandler : IRequestHandler<DeleteVehicleTypeCommand, DeleteVehicleTypeResponse>
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public DeleteVehicleTypeCommandHandler(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<DeleteVehicleTypeResponse> Handle(
            DeleteVehicleTypeCommand request,
            CancellationToken cancellationToken)
        {
            var vehicleType = await _vehicleTypeRepository.GetByIdWithVehiclesAsync(request.Id, cancellationToken);
            if (vehicleType == null)
            {
                throw new KeyNotFoundException($"Vehicle type with ID '{request.Id}' not found.");
            }

            // Check if there are vehicles using this type
            if (vehicleType.Vehicles.Any())
            {
                // Instead of deleting, deactivate
                vehicleType.Deactivate();
                await _vehicleTypeRepository.UpdateAsync(vehicleType, cancellationToken);
                return new DeleteVehicleTypeResponse
                {
                    Success = true,
                    Message = "Vehicle type has been deactivated because it has associated vehicles."
                };
            }

            await _vehicleTypeRepository.DeleteAsync(request.Id, cancellationToken);

            return new DeleteVehicleTypeResponse
            {
                Success = true,
                Message = "Vehicle type deleted successfully."
            };
        }
    }
}
