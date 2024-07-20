using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Fakebook.Application.CQRS.Posts.Queries;

public class GetPostComments : IRequest<Response<List<PostComment>>>
{
    public Guid PostId { get; set; }
}