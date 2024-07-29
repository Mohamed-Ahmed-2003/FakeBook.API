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

namespace Fakebook.Application.CQRS.Posts.CommandHandlers
{
    public class CreatePostCmdHandler : IRequestHandler<CreatePostCmd, Response<Post>>
    {
        private readonly DataContext _context;
        private readonly MediaService _mediaService;

        public CreatePostCmdHandler(DataContext context, MediaService mediaService)
        {
            _context = context;
            _mediaService = mediaService;
        }

        public async Task<Response<Post>> Handle(CreatePostCmd request, CancellationToken cancellationToken)
        {
            var result = new Response<Post>();
            try
            {
                var post = Post.CreatePost(request.UserProfileId, request.Text);
                if (request.MediaFiles != null)
                {

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
                    }
                    else
                    {
                        result.AddError(StatusCodes.UnknownError, "Media upload failed.");
                    }
                }
                }

                await _context.Set<Post>().AddAsync(post);
                await _context.SaveChangesAsync();
                result.Payload = post;
            }
            catch (PostNotValidException ex)
            {
                ex.ValidationErrors.ForEach(error =>
                {
                    result.AddError(StatusCodes.ValidationError, error);
                });
            }
            catch (Exception ex)
            {
                result.AddError(StatusCodes.UnknownError, ex.Message);
            }

            return result;
        }
    }
}
