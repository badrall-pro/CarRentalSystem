using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Features.Auth.Queries.GetCurrentUser
{
    public record GetCurrentUserQuery(Guid UserId) : IRequest<CurrentUserResponse>;
        
    public record CurrentUserResponse
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string UserType { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
   
}
