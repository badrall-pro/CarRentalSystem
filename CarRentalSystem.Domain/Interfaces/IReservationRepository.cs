using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enums;

namespace CarRentalSystem.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Reservation?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Reservation>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Reservation>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status, CancellationToken cancellationToken = default);

        Task<IEnumerable<Reservation>> GetActiveReservationsAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Reservation>> GetOverlappingReservationsAsync(
            Guid vehicleId, 
            DateTime startDate, 
            DateTime endDate, 
            Guid? excludeReservationId = null,
            CancellationToken cancellationToken = default);

        Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default);

        Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
