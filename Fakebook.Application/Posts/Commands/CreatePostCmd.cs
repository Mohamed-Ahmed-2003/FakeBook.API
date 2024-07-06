using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;


namespace Fakebook.Application.Posts.Commands
{
    public class CreatePostCmd : IRequest<Response<Post>>
    {
        public Guid UserProfileId { get; set; }
        public required string Text { get; set; }
    }
}
