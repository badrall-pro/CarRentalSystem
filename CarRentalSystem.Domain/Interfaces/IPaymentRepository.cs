using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enums;

namespace CarRentalSystem.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Payment?> GetByIdWithReservationAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Payment>> GetByReservationIdAsync(Guid reservationId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default);

        Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        Task<decimal> GetTotalAmountByReservationIdAsync(Guid reservationId, CancellationToken cancellationToken = default);

        Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken = default);

        Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
