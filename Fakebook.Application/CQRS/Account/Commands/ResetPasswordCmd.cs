using Fakebook.Application.Generics;
using MediatR;

namespace Fakebook.Application.CQRS.Account.Commands
{
    public class ResetPasswordCmd : IRequest<Response<object>>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
