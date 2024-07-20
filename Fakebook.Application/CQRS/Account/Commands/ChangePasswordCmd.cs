using Fakebook.Application.Generics;
using MediatR;
using System.Security.Claims;

namespace Fakebook.Application.CQRS.Account.Commands
{
    public class ChangePasswordCmd : IRequest<Response<object>>
    {
        public required Guid UserId{ get; set; }
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }

}
