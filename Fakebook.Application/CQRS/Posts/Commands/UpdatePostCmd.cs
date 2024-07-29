using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace Fakebook.Application.CQRS.Posts.Commands
{
    public class UpdatePostCmd : IRequest<Response<Post>>
    {
        public string NewText { get; set; }
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
        public List<IFormFile>? MediaFiles { get; set; }

    }
}