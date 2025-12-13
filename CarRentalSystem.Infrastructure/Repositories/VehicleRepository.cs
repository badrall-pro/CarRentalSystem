using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Interfaces;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly CarRentalDbContext _context;

        public VehicleRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<Vehicle?> GetByIdWithTypeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate.ToUpper(), cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> GetAllWithTypeAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .Include(v => v.VehicleType)
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .Where(v => v.Status == status)
                .Include(v => v.VehicleType)
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> GetByVehicleTypeIdAsync(Guid vehicleTypeId, CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .Where(v => v.VehicleTypeId == vehicleTypeId)
                .Include(v => v.VehicleType)
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
             var reservedVehicleIds = await _context.Reservations
            .Where(r => r.Status != ReservationStatus.Cancelled &&
                       r.Status != ReservationStatus.Completed &&
                       ((r.StartDate <= endDate && r.EndDate >= startDate)))
            .Select(r => r.VehicleId)
            .ToListAsync(cancellationToken); 

            return await _context.Vehicles
                .Where(v => v.Status == VehicleStatus.Available && !reservedVehicleIds.Contains(v.Id))
                .Include(v => v.VehicleType)
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsAvailableAsync(Guid vehicleId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var vehicle= await GetByIdAsync(vehicleId, cancellationToken);
            if(vehicle == null || vehicle.Status != VehicleStatus.Available)
                return false;

            var hasConflict = await _context.Reservations
                .AnyAsync(r => r.VehicleId == vehicleId &&
                r.Status != ReservationStatus.Cancelled &&
                r.Status != ReservationStatus.Completed &&
                ((r.StartDate <= endDate && r.EndDate >= startDate))
                , cancellationToken);
            
            return !hasConflict;
        }

        public async Task<bool> LicensePlateExistsAsync(string licensePlate, CancellationToken cancellationToken = default)
        {
            return await _context.Vehicles
                .AnyAsync(v => v.LicensePlate == licensePlate.ToUpper(), cancellationToken);
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            await _context.Vehicles.AddAsync(vehicle, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return vehicle;
        }

        public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var vehicle = await GetByIdAsync(id, cancellationToken);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
