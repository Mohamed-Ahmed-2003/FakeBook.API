﻿using Fakebook.Application.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Fakebook.Application.Account.CommandHandlers
{
    public class DeleteUserCmdHandler(DataContext context, UserManager<IdentityUser> userManager) : IRequestHandler<DeleteUserCmd, Response<UserProfile>>
    {
        private readonly DataContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        public async Task<Response<UserProfile>> Handle(DeleteUserCmd request, CancellationToken cancellationToken)
        {
            var result = new Response<UserProfile>();
           
                var userProfile = await _context.userProfiles.FindAsync(request.UserProfileId);

                var user = userProfile != null ? await _userManager.FindByIdAsync(userProfile.IdentityId) : null;

                if (user is null || userProfile is null)
                {
                    result.AddError(Generics.Enums.StatusCode.NotFound, string.Format(AccountErrorMessages.AccountNotFound, request.UserProfileId));
                    return result;
                }


                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    _context.Set<UserProfile>().Remove(userProfile);
                var identityResult = await _userManager.DeleteAsync(user);

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