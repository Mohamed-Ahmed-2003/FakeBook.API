using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Fakebook.Application.CQRS.Profile.Commands
{
    public class DeleteUserProfileCmd(Guid id) : IRequest<Response<UserProfile>>
    {
        public Guid UserProfileId { get; set; } = id;

    }
}
