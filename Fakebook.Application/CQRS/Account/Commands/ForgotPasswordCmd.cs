
using Fakebook.Application.Generics;
using MediatR;

namespace Fakebook.Application.CQRS.Account.Commands
{
    public class ForgotPasswordCmd : IRequest<Response<object>>
    {
        public required string Email { get; set; }
    }
}
