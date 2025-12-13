using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Enums;
using CarRentalSystem.Domain.Interfaces;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarRentalDbContext _context;

        public UserRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id,CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email.ToLower(), cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .OrderBy(u => u.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetByTypeAsync(UserType userType, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.UserType == userType)
                .OrderBy(u => u.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email.ToLower(), cancellationToken);
        }

        // ===== COMMAND OPERATIONS =====

        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await GetByIdAsync(id, cancellationToken);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
