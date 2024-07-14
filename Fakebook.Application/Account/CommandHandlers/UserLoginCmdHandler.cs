using Fakebook.Application.Account.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Account.CommandHandlers
{
    public class UserLoginCmdHandler(DataContext context, UserManager<IdentityUser> userManager, JwtService jwtService) : IRequestHandler<UserLoginCmd, Response<string>>
    {

        private readonly DataContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly JwtService _jwtService = jwtService;

        public async Task<Response<string>> Handle(UserLoginCmd request, CancellationToken cancellationToken)
        {
            var result = new Response<string>();

            var user = await _userManager.FindByEmailAsync(request.Username);



            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                result.Success = false;
                result.AddError(Generics.Enums.StatusCode.UserNotFound, AccountErrorMessages.WrongCredentials);
                return result;
            }

            var profile = await _context.Set<UserProfile>().FirstOrDefaultAsync(u => u.IdentityId == user.Id);
            if (profile is null)
            {
                result.Success = false;
                result.AddError(Generics.Enums.StatusCode.ProfileNotFound, AccountErrorMessages.AccountNotFound);
                return result;
            }
            // handling the token vaildation
            var token = _jwtService.GenerateJwtToken(user, profile.UserProfileId);
            result.Payload = token;

            return result;
        }
    }
}
