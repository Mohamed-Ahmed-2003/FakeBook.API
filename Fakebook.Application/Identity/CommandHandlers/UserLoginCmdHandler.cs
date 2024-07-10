using Fakebook.Application.Generics;
using Fakebook.Application.Identity.Commands;
using Fakebook.Application.Options;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Fakebook.Application.Identity.CommandHandlers
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
                result.AddError(Generics.Enums.StatusCode.UserNotFound, "Username/email or password is wrong.");
                return result;
            }

            var profile = await _context.Set<UserProfile>().FirstOrDefaultAsync(u=>u.IdentityId == user.Id);
            if (profile is  null)
            {
                result.Success = false;
                result.AddError(Generics.Enums.StatusCode.ProfileNotFound, "no profile was found with this account.");
                return result;
            }
            // handling the token vaildation
            var token = _jwtService.GenerateJwtToken(user, profile.UserProfileId);
            result.Payload = token;

            return result;
        }
    }
}
