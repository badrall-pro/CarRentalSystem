using CarRentalSystem.Application.Common.Interfaces;
using CarRentalSystem.Application.Common.Models;
using CarRentalSystem.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Role, user.UserType.ToString()),
                new("UserType", user.UserType.ToString())
            };

            var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("JWT secret is not configured");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
                signingCredentials: credentials
            );
            Console.WriteLine("=================================|||||||||||||||||||||||||||||||||||||||||||||||||||||||=====================================");
            Console.WriteLine($"SECRET USED TO SIGN === {secret}");


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
