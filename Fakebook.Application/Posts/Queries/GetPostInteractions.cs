using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Fakebook.Application.Posts.Queries;

    public class GetPostInteractions : IRequest<Response<List<PostInteraction>>>
    {
        public Guid PostId { get; set; }
    }

