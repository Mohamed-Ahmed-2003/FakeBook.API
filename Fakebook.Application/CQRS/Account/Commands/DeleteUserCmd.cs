using Fakebook.Application.Generics;
using MediatR;


namespace Fakebook.Application.CQRS.Account.Commands
{
    public class DeleteUserCmd : IRequest<Response<Unit>>
    {
        public Guid UserProfileId { get; set; }
    }
}