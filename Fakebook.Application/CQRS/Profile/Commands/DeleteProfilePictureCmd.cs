using MediatR;
using Fakebook.Application.Generics;
using Fakebook.Application.Services;
using Fakebook.DAL;

namespace Fakebook.Application.CQRS.Profile.Commands
{
    public class DeleteProfilePictureCmd : IRequest<Response<Unit>>
    {
        public Guid UserProfileId { get; set; }
    }
    public class DeleteProfilePictureCmdHandler : IRequestHandler<DeleteProfilePictureCmd, Response<Unit>>
    {
        private readonly DataContext _context;
        private readonly MediaService _mediaService;

        public DeleteProfilePictureCmdHandler(DataContext context, MediaService mediaService)
        {
            _context = context;
            _mediaService = mediaService;
        }

        public async Task<Response<Unit>> Handle(DeleteProfilePictureCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<Unit>();

            try
            {
                var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

                if (userProfile == null)
                {
                    response.AddError(Generics.Enums.StatusCodes.NotFound, "User profile not found");
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

                    userProfile.RemoveProfilePicture();
                    _context.UserProfiles.Update(userProfile);
                    await _context.SaveChangesAsync(cancellationToken);
                }

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
