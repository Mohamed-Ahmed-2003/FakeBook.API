using Fakebook.Application.CQRS.Account.Dtos;
using Fakebook.Application.Generics;
using MediatR;


namespace Fakebook.Application.CQRS.Account.Commands
{
    public class UserLoginCmd : IRequest<Response<IdentityUserProfileDto>>
    {
        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}
