using CarRentalSystem.Application.Common.Interfaces;
using CarRentalSystem.Application.Common.Models;
using CarRentalSystem.Application.Features.Customers.Commands.RegisterCustomer;
using CarRentalSystem.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(
                request.Email,
                cancellationToken);
            if (user==null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Account is deactivated");
            }

            var isPasswordValid = _passwordHasher.Verify(
                request.Password,
                user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = _jwtTokenService.GenerateToken(user);

            var expiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours);

            return new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                UserType = user.UserType.ToString(),
                ExpiresAt = expiresAt
            };
        }
    }

    
}
