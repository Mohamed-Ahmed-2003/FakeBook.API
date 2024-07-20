using Fakebook.Application.CQRS.Posts;
using Fakebook.Application.CQRS.Posts.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Posts.CommandHandlers;

public class RemoveCommentFromPostHandler : IRequestHandler<RemovePostCommentCmd, Response<PostComment>>
{
    private readonly DataContext _ctx;

    public RemoveCommentFromPostHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Response<PostComment>> Handle(RemovePostCommentCmd request, CancellationToken cancellationToken)
    {
        var result = new Response<PostComment>();

        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post == null)
            {
                result.AddError(StatusCode.NotFound, string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            var comment = post.Comments.FirstOrDefault(c => c.CommentId == request.CommentId);
            if (comment == null)
            {
                result.AddError(StatusCode.NotFound, PostsErrorMessages.PostCommentNotFound);
                return result;
            }

            if (comment.UserProfileId != request.UserProfileId)
            {
                result.AddError(StatusCode.CommentRemovalNotAuthorized, PostsErrorMessages.CommentRemovalNotAuthorized);
                return result;
            }

            post.RemoveComment(comment);
            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = comment;
        }
        catch (PostCommentNotValidException e)
        {
            e.ValidationErrors.ForEach(er => result.AddError(StatusCode.ValidationError, er));
        }
        catch (Exception e)
        {
            result.AddError(StatusCode.Unknown, e.Message);
        }

        return result;
    }
}
