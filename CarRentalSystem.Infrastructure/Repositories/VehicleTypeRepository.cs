using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Interfaces;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Repositories
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly CarRentalDbContext _context;

        public VehicleTypeRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<VehicleType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.VehicleTypes
                .FirstOrDefaultAsync(vt => vt.Id == id, cancellationToken);
        }

        public async Task<VehicleType?> GetByIdWithVehiclesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.VehicleTypes
                .Include(vt => vt.Vehicles)
                .FirstOrDefaultAsync(vt => vt.Id == id, cancellationToken);
        }

        public async Task<VehicleType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.VehicleTypes
                .FirstOrDefaultAsync(vt => vt.Name.ToLower() == name.ToLower(), cancellationToken);
        }

        public async Task<IEnumerable<VehicleType>> GetAllAsync(bool includeInactive = false, CancellationToken cancellationToken = default)
        {
            var query=_context.VehicleTypes.AsQueryable();
            if(!includeInactive)
            {
                query=query.Where(vt=>vt.IsActive);
            }
            return await query
                .OrderBy(vt => vt.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<VehicleType>> GetActiveTypesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.VehicleTypes
                .Where(vt => vt.IsActive)
                .OrderBy(vt => vt.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.VehicleTypes
                .AnyAsync(vt => vt.Name.ToLower() == name.ToLower(), cancellationToken);
        }

        public async Task<VehicleType> AddAsync(VehicleType vehicleType, CancellationToken cancellationToken = default)
        {
            await _context.VehicleTypes.AddAsync(vehicleType, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return vehicleType;
        }

        public async Task UpdateAsync(VehicleType vehicleType, CancellationToken cancellationToken = default)
        {
            _context.VehicleTypes.Update(vehicleType);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var vehicleType = await GetByIdAsync(id, cancellationToken);
            if (vehicleType != null)
            {
                _context.VehicleTypes.Remove(vehicleType);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
