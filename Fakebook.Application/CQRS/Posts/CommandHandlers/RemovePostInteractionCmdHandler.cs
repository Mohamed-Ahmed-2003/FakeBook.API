using Fakebook.Application.CQRS.Posts;
using Fakebook.Application.CQRS.Posts.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Posts.CommandHandlers;

public class RemovePostInteractionCmdHandler : IRequestHandler<RemovePostInteractionCmd, Response<PostInteraction>>
{
    private readonly DataContext _ctx;

    public RemovePostInteractionCmdHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Response<PostInteraction>> Handle(RemovePostInteractionCmd request,
        CancellationToken cancellationToken)
    {
        var result = new Response<PostInteraction>();
        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Interactions)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post is null)
            {
                result.AddError(StatusCodes.NotFound,
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            var interaction = post.Interactions.FirstOrDefault(i
                => i.InteractionId == request.InteractionId);

            if (interaction == null)
            {
                result.AddError(StatusCodes.NotFound, PostsErrorMessages.PostInteractionNotFound);
                return result;
            }

            if (interaction.UserProfileId != request.UserProfileId)
            {
                result.AddError(StatusCodes.InteractionRemovalNotAuthorized,
                    PostsErrorMessages.InteractionRemovalNotAuthorized);
                return result;
            }

            post.RemoveReaction(interaction);
            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = interaction;
        }
        catch (Exception e)
        {
            result.AddError(StatusCodes.UnknownError, e.Message);
        }

        return result;
    }
}