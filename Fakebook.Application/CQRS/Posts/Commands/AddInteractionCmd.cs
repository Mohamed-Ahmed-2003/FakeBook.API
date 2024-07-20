using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Fakebook.Application.CQRS.Posts.Commands;

public class AddInteractionCmd : IRequest<Response<PostInteraction>>
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public ReactionType Type { get; set; }
}