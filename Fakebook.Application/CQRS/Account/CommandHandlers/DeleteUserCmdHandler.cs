using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Services;
using Fakebook.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Fakebook.Application.CQRS.Account.CommandHandlers
{
    public class DeleteUserCmdHandler : IRequestHandler<DeleteUserCmd, Response<Unit>>
    {
        private readonly DataContext _context;
        private readonly MediaService _mediaService;
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteUserCmdHandler(DataContext context, MediaService mediaService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mediaService = mediaService;
            _userManager = userManager;
        }

        public async Task<Response<Unit>> Handle(DeleteUserCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<Unit>();

            var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);

            if (userProfile is null)
            {
                response.AddError(Generics.Enums.StatusCodes.NotFound, "User profile not found.");
                return response;
            }

            var user = await _userManager.FindByIdAsync(userProfile.IdentityId);

            if (user is null)
            {
                response.AddError(Generics.Enums.StatusCodes.NotFound, "User not found.");
                return response;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Delete profile picture and cover image if they exist
                if (userProfile.ProfilePicture != null)
                {
                    await _mediaService.DeletePhotoAsync(userProfile.ProfilePicture.PublicId);
                }

                if (userProfile.ProfileCoverImage != null)
                {
                    await _mediaService.DeletePhotoAsync(userProfile.ProfileCoverImage.PublicId);
                }

                // Remove the user profile and the user from the system
                _context.UserProfiles.Remove(userProfile);
                var identityResult = await _userManager.DeleteAsync(user);

                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        response.AddError(Generics.Enums.StatusCodes.ValidationError, error.Description);
                    }

                    await transaction.RollbackAsync();
                    return response;
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();

                response.Payload = Unit.Value;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.AddError(Generics.Enums.StatusCodes.UnknownError, ex.Message);
            }

            return response;
        }
    }
}
