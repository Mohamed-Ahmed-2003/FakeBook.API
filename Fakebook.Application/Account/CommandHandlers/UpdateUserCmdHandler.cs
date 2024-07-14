﻿using AutoMapper;
using Fakebook.Application.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;


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

            if (userProfile is null || user is null)
            {
                result.AddError(Generics.Enums.StatusCode.NotFound, string.Format(AccountErrorMessages.AccountNotFound, request.UserProfileId));
                return result;
            }


            if (request.EmailAddress != user.Email) {
                user.Email = request.EmailAddress;
                user.UserName = request.EmailAddress;
            }
                user.PhoneNumber = request.Phone;

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
                result.Payload = userProfile;
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