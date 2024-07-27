using Fakebook.Application.CQRS.Profile.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;


namespace Fakebook.Application.CQRS.Profile.CommandHandlers
{
    public class DeleteUserProfileCmdHandler(DataContext context, MediaService mediaService) : IRequestHandler<DeleteUserProfileCmd, Response<UserProfile>>
    {
        private readonly DataContext _context = context;
        private readonly MediaService _mediaService = mediaService;

        public async Task<Response<UserProfile>> Handle(DeleteUserProfileCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<UserProfile>();

            var userProfile = await _context.Set<UserProfile>().FindAsync(request.UserProfileId, cancellationToken);

            if (userProfile is null)
            {
                response.Errors.Add(new ErrorResult { Status = Generics.Enums.StatusCodes.NotFound, Message = "User Profile is not exist" });
            }
            else
            {
                if (userProfile.ProfilePicture != null)
                {
                    await _mediaService.DeletePhotoAsync(userProfile.ProfilePicture.PublicId);
                    userProfile.RemoveProfilePicture();
                }
                  
                
                if (userProfile.ProfileCoverImage != null)
                {
                    await _mediaService.DeletePhotoAsync(userProfile.ProfileCoverImage.PublicId);
                    userProfile.RemoveProfileCoverImage();
                }

                _context.Set<UserProfile>().Remove(userProfile);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return response;
        }
    }

}
