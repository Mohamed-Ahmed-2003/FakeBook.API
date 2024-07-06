using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Fakebook.Application.Posts.Commands
{
    public class DeletePostCmd : IRequest<Response<Post>>
    {
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
    }
}