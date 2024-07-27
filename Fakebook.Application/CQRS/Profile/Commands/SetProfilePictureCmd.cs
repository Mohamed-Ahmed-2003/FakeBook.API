using AutoMapper;
using Fakebook.Application.CQRS.Account;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Fakebook.Application.CQRS.Profile.Commands
{
    public class SetProfilePictureCmd : IRequest<Response<Unit>>
    {
        public Guid UserProfileId { get; set; }
        public IFormFile FormFile { get; set; }
        }

    public class SetProfilePictureCmdHandler : IRequestHandler<SetProfilePictureCmd, Response<Unit>>
    {
        private readonly DataContext _context;
        private readonly MediaService _mediaService;

        public SetProfilePictureCmdHandler(DataContext context, MediaService mediaService)
        {
            _context = context;
            _mediaService = mediaService;
        }

        public async Task<Response<Unit>> Handle(SetProfilePictureCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<Unit>();

            try
            {
                var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

                if (userProfile == null)
                {
                    response.AddError(Generics.Enums.StatusCodes.NotFound, AccountErrorMessages.AccountNotFound);
                    return response;
                }

                // Remove existing profile picture if it exists
                if (userProfile.ProfilePicture != null)
                {
                    var deleteResult = await _mediaService.DeletePhotoAsync(userProfile.ProfilePicture.PublicId);
                    if (deleteResult.Error != null)
                    {
                        response.AddError(Generics.Enums.StatusCodes.ImageDeletionFailed, "Failed to delete existing profile picture");
                        return response;
                    }
                }

                // Upload new profile picture
                var img = await _mediaService.AddPhotoAsync(request.FormFile);

                if (img == null)
                {
                    response.AddError(Generics.Enums.StatusCodes.ImageUploadFailed, "Image upload failed");
                    return response;
                }

                var media = Media.CreateMedia(img.PublicId,img.SecureUrl.AbsoluteUri, MediaType.Image);

                userProfile.SetProfilePicture(media);

                _context.UserProfiles.Update(userProfile);
                await _context.SaveChangesAsync(cancellationToken);

                response.Payload = Unit.Value;
            }
            catch (Exception ex)
            {
                response.AddError(Generics.Enums.StatusCodes.UnknownError, "An error occurred: " + ex.Message);
            }

            return response;
        }
    }



}
