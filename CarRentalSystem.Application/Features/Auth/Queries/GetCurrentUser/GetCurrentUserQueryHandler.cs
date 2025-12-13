using CarRentalSystem.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Features.Auth.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery,CurrentUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetCurrentUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CurrentUserResponse> Handle(
            GetCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if(user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            return new CurrentUserResponse
            {
                Id = user.Id,
                FirstName=user.FirstName,
                LastName=user.LastName,
                Email=user.Email,
                UserType=user.UserType.ToString(),
                IsActive=user.IsActive
            };
        }
    }
}
