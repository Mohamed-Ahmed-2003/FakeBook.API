using Fakebook.Application.CQRS.Posts.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Posts.CommandHandlers;

public class DeletePostCmdHandler(DataContext ctx, MediaService mediaService) : IRequestHandler<DeletePostCmd, Response<Post>>
{
    private readonly DataContext _ctx = ctx;
    private readonly MediaService _mediaService = mediaService;

    public async Task<Response<Post>> Handle(DeletePostCmd request, CancellationToken cancellationToken)
    {
        var result = new Response<Post>();
        try
        {
            var post = await _ctx.Posts.Include(p=>p.Comments)
                .Include(p=>p.Interactions).FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);

            if (post is null)
            {
                result.AddError(StatusCodes.NotFound,
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));

                return result;
            }

            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(StatusCodes.PostDeleteNotPossible, PostsErrorMessages.PostDeleteNotPossible);
                return result;
            }


            foreach (var media in post.PostMedia)
            {
                await _mediaService.DeleteMediaAsync(media.PublicId);
            }
            _ctx.Posts.Remove(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = post;
        }
        catch (Exception e)
        {
            result.AddError(StatusCodes.UnknownError, e.Message);
        }

        return result;
    }
}