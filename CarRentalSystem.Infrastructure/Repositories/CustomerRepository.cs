using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Interfaces;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CarRentalDbContext _context;

        public CustomerRepository(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Customer?> GetByLicenseNumberAsync(string licenseNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.LicenseNumber == licenseNumber.ToUpper(), cancellationToken);
        }

        public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .OrderBy(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetEligibleCustomersAsync(CancellationToken cancellationToken = default)
        {
            var eighteenYearsAgo = DateTime.UtcNow.AddYears(-18);
            return await _context.Customers
                .Where(c => c.DateOfBirth <= eighteenYearsAgo && c.IsActive)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> LicenseNumberExistsAsync(string licenseNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .AnyAsync(c => c.LicenseNumber == licenseNumber.ToUpper(), cancellationToken);
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .AnyAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
        }

        public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return customer;
        }

        public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var customer = await GetByIdAsync(id, cancellationToken);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
