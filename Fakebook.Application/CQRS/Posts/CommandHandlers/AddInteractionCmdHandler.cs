using Fakebook.Application.CQRS.Posts;
using Fakebook.Application.CQRS.Posts.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Posts.CommandHandlers;

public class AddInteractionCmdHandler : IRequestHandler<AddInteractionCmd, Response<PostInteraction>>
{
    private readonly DataContext _ctx;

    public AddInteractionCmdHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Response<PostInteraction>> Handle(AddInteractionCmd request, CancellationToken cancellationToken)
    {
        var result = new Response<PostInteraction>();
        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Interactions)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post == null)
            {
                result.AddError(StatusCode.NotFound, PostsErrorMessages.PostNotFound);
                return result;
            }

            var prevInteraction = post.Interactions
                .FirstOrDefault(i => i.UserProfileId == request.UserProfileId);

            if (prevInteraction != null)
            {
                post.RemoveReaction(prevInteraction);
            }

            var interaction = PostInteraction.CreatePostInteraction(request.PostId, request.UserProfileId, request.Type);
            post.AddReaction(interaction);

            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = interaction;
        }
        catch (Exception e)
        {
            result.AddError(StatusCode.Unknown, e.Message);
        }

        return result;
    }

}