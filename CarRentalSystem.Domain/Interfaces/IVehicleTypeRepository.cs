using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Domain.Interfaces
{
    public interface IVehicleTypeRepository
    {
        Task<VehicleType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<VehicleType?> GetByIdWithVehiclesAsync(Guid id, CancellationToken cancellationToken = default);

        Task<VehicleType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<IEnumerable<VehicleType>> GetAllAsync(bool includeInactive = false, CancellationToken cancellationToken = default);

        Task<IEnumerable<VehicleType>> GetActiveTypesAsync(CancellationToken cancellationToken = default);

        Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);

        Task<VehicleType> AddAsync(VehicleType vehicleType, CancellationToken cancellationToken = default);

        Task UpdateAsync(VehicleType vehicleType, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
