using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Features.Auth.Commands.Login
{
    public record LoginCommand : IRequest<LoginResponse>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }

    public record LoginResponse {
        public string Token { get; init; } = string.Empty;
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public string UserType { get; init; } = string.Empty;
        public DateTime ExpiresAt { get; init; }
    }
}
