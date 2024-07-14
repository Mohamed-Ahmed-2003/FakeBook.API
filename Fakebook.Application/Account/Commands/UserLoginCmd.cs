using Fakebook.Application.Generics;
using MediatR;


namespace Fakebook.Application.Account.Commands
{
    public class UserLoginCmd : IRequest<Response<string>>
    {
        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}
