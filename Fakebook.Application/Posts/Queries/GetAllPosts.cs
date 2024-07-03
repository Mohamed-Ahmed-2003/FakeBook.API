
using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Fakebook.Application.Posts.Queries
{
    public class GetAllPosts : IRequest<Response<List<Post>>>
    {
    }
}
