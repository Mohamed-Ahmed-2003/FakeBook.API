using AutoMapper;
using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.CQRS.Account.Dtos;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Fakebook.Application.CQRS.Account.CommandHandlers
{
    public class UpdateUserCmdHandler(DataContext context, UserManager<IdentityUser> userManager, IMapper mapper) : IRequestHandler<UpdateUserCmd, Response<IdentityUserProfileDto>>
    {
        private readonly DataContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<IdentityUserProfileDto>> Handle(UpdateUserCmd request, CancellationToken cancellationToken)
        {
            var result = new Response<IdentityUserProfileDto>();

            var userProfile = await _context.UserProfiles.FindAsync(request.UserProfileId);
            var user = userProfile != null ? await _userManager.FindByIdAsync(userProfile.IdentityId) : null;

            if (userProfile is null || user is null)
            {
                result.AddError(Generics.Enums.StatusCode.NotFound, string.Format(AccountErrorMessages.AccountNotFound, request.UserProfileId));
                return result;
            }
            var updatedInfo = GeneralInfo.CreateBasicInfo(request.FirstName, request.LastName,
                request.EmailAddress, request.Phone ??userProfile.GeneralInfo.Phone, request.DateOfBirth, request.City??userProfile.GeneralInfo.City);

            userProfile.UpdateBasicInfo(updatedInfo);

            if (request.EmailAddress != user.Email)
            {
                user.Email = request.EmailAddress;
                user.UserName = request.EmailAddress;
            }

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
                var mapped = _mapper.Map<IdentityUserProfileDto>(userProfile);
                mapped.UserName = user.UserName;
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
                result.Payload = mapped;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.AddError(Generics.Enums.StatusCode.Unknown, ex.Message);
            }

            return result;
        }

    }
}
