using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Interfaces;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CarRentalDbContext _context;

        public PaymentRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Payment?> GetByIdWithReservationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Customer)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Vehicle)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetByReservationIdAsync(Guid reservationId, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .Where(p => p.ReservationId == reservationId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .Where(p => p.Status == status)
                .Include(p => p.Reservation)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .Include(p => p.Reservation)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalAmountByReservationIdAsync(Guid reservationId, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .Where(p => p.ReservationId == reservationId && p.Status == PaymentStatus.Completed)
                .SumAsync(p => p.Amount, cancellationToken);
        }

        public async Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken = default)
        {
            await _context.Payments.AddAsync(payment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return payment;
        }

        public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var payment = await GetByIdAsync(id, cancellationToken);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
