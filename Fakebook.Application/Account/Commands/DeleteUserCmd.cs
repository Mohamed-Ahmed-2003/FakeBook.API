using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;


namespace Fakebook.Application.Account.Commands
{
    public class DeleteUserCmd : IRequest<Response<UserProfile>>
    {
        public Guid UserProfileId { get; set; }
    }
}