using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enums;

namespace CarRentalSystem.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Vehicle?> GetByIdWithTypeAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Vehicle?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default);

        Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Vehicle>> GetAllWithTypeAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status, CancellationToken cancellationToken = default);

        Task<IEnumerable<Vehicle>> GetByVehicleTypeIdAsync(Guid vehicleTypeId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        Task<bool> IsAvailableAsync(Guid vehicleId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        Task<bool> LicensePlateExistsAsync(string licensePlate, CancellationToken cancellationToken = default);

        Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);

        Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
