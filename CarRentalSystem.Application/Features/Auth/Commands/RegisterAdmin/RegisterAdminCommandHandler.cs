using CarRentalSystem.Application.Common.Interfaces;
using CarRentalSystem.Application.Features.Customers.Commands.RegisterCustomer;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Features.Auth.Commands.RegisterAdmin
{
    public class RegisterAdminrCommandHandler : IRequestHandler<RegisterAdminCommand, RegisterAdminResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public RegisterAdminrCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
            {
                _userRepository = userRepository;
                _passwordHasher = passwordHasher;
            }
        public async Task<RegisterAdminResponse> Handle(
                RegisterAdminCommand request,
                CancellationToken cancellationToken)
            {
                // Step 1: Check if email already exists
                var emailExists = await _userRepository.EmailExistsAsync(
                    request.Email,
                    cancellationToken);

                if (emailExists)
                {
                    throw new InvalidOperationException(
                        $"A user with email '{request.Email}' already exists.");
                }

                // Step 2: Hash the password
                var passwordHash = _passwordHasher.Hash(request.Password);

                // Step 3: Create the admin
                var admin = User.CreateAdministrator(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    passwordHash
                    );

                // Step 4: Save to database
                await _userRepository.AddAsync(admin, cancellationToken);

                // Step 5: Return the response
                return new RegisterAdminResponse
                {
                    Id = admin.Id,
                    Email = admin.Email,
                    FullName = admin.FullName,
                    CreatedAt = admin.CreatedAt
                };
            }

            

            
   }
}

