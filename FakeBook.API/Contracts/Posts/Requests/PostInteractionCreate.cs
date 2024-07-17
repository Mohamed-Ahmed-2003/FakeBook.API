using FakeBook.Domain.Aggregates.PostAggregate;
namespace FakeBook.API.Contracts.Posts.Requests;
public class PostInteractionCreate
{
    public ReactionType Type { get; set; }
}