using MediatR;
using Fakebook.Application.Generics;
using Fakebook.Application.Services;
using Fakebook.DAL;

namespace Fakebook.Application.CQRS.Profile.Commands
{
    public class DeleteProfileCoverImageCmd : IRequest<Response<Unit>>
    {
        public Guid UserProfileId { get; set; }
    }
    public class DeleteProfileCoverImageCmdHandler : IRequestHandler<DeleteProfileCoverImageCmd, Response<Unit>>
    {
        private readonly DataContext _context;
        private readonly MediaService _mediaService;

        public DeleteProfileCoverImageCmdHandler(DataContext context, MediaService mediaService)
        {
            _context = context;
            _mediaService = mediaService;
        }

        public async Task<Response<Unit>> Handle(DeleteProfileCoverImageCmd request, CancellationToken cancellationToken)
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

                // Remove existing cover image if it exists
                if (userProfile.ProfileCoverImage != null)
                {
                    var deleteResult = await _mediaService.DeletePhotoAsync(userProfile.ProfileCoverImage.PublicId);
                    if (deleteResult.Error != null)
                    {
                        response.AddError(Generics.Enums.StatusCodes.ImageDeletionFailed, "Failed to delete existing cover image");
                        return response;
                    }

                    userProfile.RemoveProfileCoverImage();
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
