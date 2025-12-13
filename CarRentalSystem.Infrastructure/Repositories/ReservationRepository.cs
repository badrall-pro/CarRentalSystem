using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Interfaces;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly CarRentalDbContext _context;

        public ReservationRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<Reservation?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                    .ThenInclude(v => v.VehicleType)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Reservation>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                    .ThenInclude(v => v.VehicleType)
                .Include(r => r.Payments)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .Where(r => r.CustomerId == customerId)
                .Include(r => r.Vehicle)
                    .ThenInclude(v => v.VehicleType)
                .Include(r => r.Payments)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Reservation>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .Where(r => r.VehicleId == vehicleId)
                .Include(r => r.Customer)
                .Include(r => r.Payments)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .Where(r => r.Status == status)
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                    .ThenInclude(v => v.VehicleType)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .Where(r => r.Status == ReservationStatus.Active)
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                    .ThenInclude(v => v.VehicleType)
                .Include(r => r.Payments)
                .OrderBy(r => r.EndDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Reservation>> GetOverlappingReservationsAsync(
            Guid vehicleId,
            DateTime startDate,
            DateTime endDate,
            Guid? excludeReservationId = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Reservations
                .Where(r => r.VehicleId == vehicleId)
                .Where(r => r.Status != ReservationStatus.Cancelled)
                .Where(r => r.StartDate < endDate && r.EndDate > startDate);

            if (excludeReservationId.HasValue)
            {
                query = query.Where(r => r.Id != excludeReservationId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default)
        {
            await _context.Reservations.AddAsync(reservation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return reservation;
        }

        public async Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var reservation = await GetByIdAsync(id, cancellationToken);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
