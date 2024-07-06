
using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Fakebook.Application.Posts.Commands;
public class AddPostCommentCmd : IRequest<Response<PostComment>>
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public string CommentText { get; set; }
}