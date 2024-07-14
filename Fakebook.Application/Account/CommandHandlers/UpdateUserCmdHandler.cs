using AutoMapper;
using Fakebook.Application.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Fakebook.Application.Account.CommandHandlers
{
    public class UpdateUserCmdHandler(DataContext context, UserManager<IdentityUser> userManager,  IMapper mapper) : IRequestHandler<UpdateUserCmd, Response<UserProfile>>
    {
        private readonly DataContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<UserProfile>> Handle(UpdateUserCmd request, CancellationToken cancellationToken)
        {
            var result = new Response<UserProfile>();

            var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);
            var user = userProfile != null ? await _userManager.FindByIdAsync(userProfile.IdentityId) : null;

            if (userProfile == null || user == null)
            {
                result.AddError(Generics.Enums.StatusCode.NotFound, string.Format(AccountErrorMessages.AccountNotFound, request.UserProfileId));
                return result;
            }

            // Update UserProfile
            _mapper.Map(request.UserProfileUpdateDto, userProfile);

            // Optionally, update IdentityUser details here if necessary
            user.Email = request.UserProfileUpdateDto.Email;
            user.UserName = request.UserProfileUpdateDto.UserName;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.UserProfiles.Update(userProfile);
                var identityResult = await _userManager.UpdateAsync(user);

                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        result.AddError(Generics.Enums.StatusCode.ValidationError, error.Description);
                    }
                    await transaction.RollbackAsync();
                    return result;
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
                result.Success = true;
                result.Payload = userProfile;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating user and profile for UserProfileId: {UserProfileId}", request.UserProfileId);
                result.AddError(Generics.Enums.StatusCode.Unknown, ex.Message);
            }

            return result;
        }
    {
    }
}
