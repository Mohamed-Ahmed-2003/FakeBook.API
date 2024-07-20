using AutoMapper;
using Fakebook.Application.CQRS.Account;
using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.CQRS.Account.Dtos;
using Fakebook.Application.Generics;
using Fakebook.Application.Services;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Account.CommandHandlers
{
    public class UserLoginCmdHandler(DataContext context, UserManager<IdentityUser> userManager, JwtService jwtService, IMapper mapper) : IRequestHandler<UserLoginCmd, Response<IdentityUserProfileDto>>
    {

        private readonly DataContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly JwtService _jwtService = jwtService;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<IdentityUserProfileDto>> Handle(UserLoginCmd request, CancellationToken cancellationToken)
        {
            var result = new Response<IdentityUserProfileDto>();

            var user = await _userManager.FindByEmailAsync(request.Username);

            

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                result.AddError(Generics.Enums.StatusCode.UserNotFound, AccountErrorMessages.WrongCredentials);
                return result;
            }

            var profile = await _context.Set<UserProfile>().FirstOrDefaultAsync(u => u.IdentityId == user.Id);
            if (profile is null)
            {
                result.AddError(Generics.Enums.StatusCode.ProfileNotFound, AccountErrorMessages.AccountNotFound);
                return result;
            }
            var mapped = _mapper.Map<IdentityUserProfileDto>(profile);
            mapped.UserName = user.UserName;

            // handling the token vaildation
            mapped.Token = _jwtService.GenerateJwtToken(user, profile.UserProfileId);

            result.Payload = mapped;

            return result;
        }
    }
}
