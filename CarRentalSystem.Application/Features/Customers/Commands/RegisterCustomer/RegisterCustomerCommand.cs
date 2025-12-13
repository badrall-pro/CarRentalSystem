using MediatR;
//using static CarRentalSystem.Application.Features.Customers.Commands.RegisterCustomer.RegisterCustomerCommand;


namespace CarRentalSystem.Application.Features.Customers.Commands.RegisterCustomer
{
    // COMMAND TO REGISTER A NEW CUSTOMER
    public record RegisterCustomerCommand : IRequest<RegisterCustomerResponse>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string LicenseNumber { get; init; } = string.Empty;
        public DateTime DateOfBirth { get; init; }
    }

    // RESPONSE AFTER REGISTRATION
    public record RegisterCustomerResponse
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
            
    }
    
}
