using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Fakebook.Application.CQRS.Posts.Commands
{
    public class CreatePostCmd : IRequest<Response<Post>>
    {
        public Guid UserProfileId { get; set; }
        public required string Text { get; set; }
        public List<IFormFile>? MediaFiles { get; set; }
    }
}
