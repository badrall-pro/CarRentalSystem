using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Customer?> GetByLicenseNumberAsync(string licenseNumber, CancellationToken cancellationToken = default);

        Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Customer>> GetEligibleCustomersAsync(CancellationToken cancellationToken = default);

        Task<bool> LicenseNumberExistsAsync(string licenseNumber, CancellationToken cancellationToken = default);

        Task<bool> PhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default);

        Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default);

        Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
