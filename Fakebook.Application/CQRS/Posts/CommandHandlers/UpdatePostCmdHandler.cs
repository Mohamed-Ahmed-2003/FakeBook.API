using CloudinaryDotNet.Actions;
using Fakebook.Application.CQRS.Posts.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using FakeBook.Domain.Aggregates.Shared;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Posts.CommandHandlers;

public class UpdatePostCmdHandler(DataContext ctx, MediaService mediaService) : IRequestHandler<UpdatePostCmd, Response<Post>>
{
    private readonly DataContext _ctx = ctx;
    private readonly MediaService _mediaService = mediaService;

   public async Task<Response<Post>> Handle(UpdatePostCmd request, CancellationToken cancellationToken)
{
    var result = new Response<Post>();

    try
    {
        var post = await _ctx.Posts
            .Include(p => p.PostMedia) // Ensure media files are included
            .AsTracking()
            .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);

        if (post is null)
        {
            result.AddError(StatusCodes.NotFound,
                string.Format(PostsErrorMessages.PostNotFound, request.PostId));
            return result;
        }

        if (post.UserProfileId != request.UserProfileId)
        {
            result.AddError(StatusCodes.PostUpdateNotPossible, PostsErrorMessages.PostUpdateNotPossible);
            return result;
        }

        post.UpdatePostText(request.NewText);

        if (request.MediaFiles != null)
        {
            foreach (var media in post.PostMedia.ToList()) // Convert to list to avoid modifying the collection during iteration
            {
                await _mediaService.DeleteMediaAsync(media.PublicId);
                post.RemoveMedia(media); // Use a method to remove a single media item
            }

            foreach (var file in request.MediaFiles)
            {
                if (!Enum.TryParse<MediaType>(file.Headers["FileFormat"].ToString(), out var mediaType))
                {
                    result.AddError(StatusCodes.ValidationError, "Invalid file format header value.");
                    return result;
                }

                var uploadResult = await _mediaService.AddMediaAsync(file, mediaType);
                if (uploadResult != null && uploadResult.Error == null)
                {
                    var mediaFile = Media.CreateMedia(uploadResult.PublicId, uploadResult.SecureUrl.AbsoluteUri, mediaType);
                    post.AddMedia(mediaFile);
                    _ctx.Entry(mediaFile).State = EntityState.Added; // Explicitly mark as added

                    }
                    else
                {
                    result.AddError(StatusCodes.UnknownError, "Media upload failed.");
                }
            }
        }
        await _ctx.SaveChangesAsync(cancellationToken);
        result.Payload = post;
    }
    catch (PostNotValidException e)
    {
        e.ValidationErrors.ForEach(er => result.AddError(StatusCodes.ValidationError, er));
    }
    catch (Exception e)
    {
        result.AddError(StatusCodes.UnknownError, e.Message);
    }

    return result;
}



}