using Fakebook.Application.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Fakebook.Application.Account.CommandHandlers
{
    public class UserRegisterCmdHandler(DataContext context, UserManager<IdentityUser> userManager, JwtService jwtService) : IRequestHandler<UserRegisterCmd, Response<string>>
    {
        private readonly DataContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly JwtService _jwtService = jwtService;

        public async Task<Response<string>> Handle(UserRegisterCmd request, CancellationToken cancellationToken)
        {
            var result = new Response<string>();

            try
            {
                // create Identity User

                var usedBefore = await _userManager.FindByEmailAsync(request.Username) != null;

                if (usedBefore)
                {
                    result.Success = false;
                    result.AddError(StatusCode.UserAlreadyExists, AccountErrorMessages.UserNameTaken);
                    return result;
                }

                var user = new IdentityUser
                {
                    Email = request.Username,
                    UserName = request.Username,
                    PhoneNumber = request.Phone,
                };
                using var transaction = await _context.Database.BeginTransactionAsync();

                var res = await _userManager.CreateAsync(user, request.Password);
                // create User Profile 
                if (!res.Succeeded)
                {
                    await transaction.RollbackAsync();
                    result.Success = false;
                    foreach (var err in res.Errors)
                    {
                        result.AddError(StatusCode.UserCreationFailed, err.Description);
                    }
                    return result;
                }
                var userInfo = GeneralInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username
                    , request.Phone, request.DateOfBirth, request.City);

                var profile = UserProfile.CreateUserProfile(user.Id, userInfo);
                try
                {
                    await _context.Set<UserProfile>().AddAsync(profile);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                // Generate the token

                result.Payload = _jwtService.GenerateJwtToken(user, profile.UserProfileId);
            }
            catch (ProfileNotValidException ex)
            {
                ex.ValidationErrors.ForEach(er => result.AddError(StatusCode.ValidationError, er));

            }
            catch (Exception ex)
            {
                result.AddError(StatusCode.Unknown, ex.Message);
            }
            return result;

        }
    }
}
