using Fakebook.Application.CQRS.Account;
using Fakebook.Application.Generics;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.Shared;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Fakebook.Application.CQRS.Profile.Commands
{
    public class SetProfileCoverImageCmd : IRequest<Response<Unit>>
    {
        public Guid UserProfileId { get; set; }
        public IFormFile FormFile {get;set;}
}

        public class SetProfileCoverImageCmdHandler : IRequestHandler<SetProfileCoverImageCmd, Response<Unit>>
        {
            private readonly DataContext _context;
            private readonly MediaService _mediaService;

            public SetProfileCoverImageCmdHandler(DataContext context, MediaService mediaService)
            {
                _context = context;
                _mediaService = mediaService;
            }

            public async Task<Response<Unit>> Handle(SetProfileCoverImageCmd request, CancellationToken cancellationToken)
            {
                var response = new Response<Unit>();

                try
                {
                    var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

                    if (userProfile is null)
                    {
                        response.AddError(Generics.Enums.StatusCodes.NotFound, AccountErrorMessages.AccountNotFound);
                        return response;
                    }

                    var img = await _mediaService.AddPhotoAsync(request.FormFile);

                    if (img is null)
                    {
                        response.AddError(Generics.Enums.StatusCodes.ImageUploadFailed, "Image upload failed");
                        return response;
                    }

                    var media = Media.CreateMedia(img.PublicId ,img.SecureUrl.AbsoluteUri, MediaType.Image);

                    userProfile.SetProfileCoverImage(media);

                    _context.UserProfiles.Update(userProfile);
                    await _context.SaveChangesAsync(cancellationToken);

                    response.Payload = Unit.Value;
                }catch (MediaNotValidException e)
            {
                foreach (var item in e.ValidationErrors)
                {
                    response.AddError(Generics.Enums.StatusCodes.ValidationError, item);
                }
            }
                catch (Exception ex)
                {
                    response.AddError(Generics.Enums.StatusCodes.UnknownError, "An error occurred: " + ex.Message);
                }

                return response;
            }
        }

    }

