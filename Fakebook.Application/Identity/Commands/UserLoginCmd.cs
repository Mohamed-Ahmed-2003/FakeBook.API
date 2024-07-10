using Fakebook.Application.Generics;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace Fakebook.Application.Identity.Commands
{
    public class UserLoginCmd : IRequest<Response<string>>
    {
        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}
