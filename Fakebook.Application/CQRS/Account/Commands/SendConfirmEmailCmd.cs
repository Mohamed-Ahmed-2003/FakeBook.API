
using Fakebook.Application.Generics;
using MediatR;
using System.Security.Claims;

namespace Fakebook.Application.CQRS.Account.Commands
{
    public class SendConfirmEmailCmd : IRequest<Response<object>>
    {
        public required Guid UserId{ get; set; }
    }
}
