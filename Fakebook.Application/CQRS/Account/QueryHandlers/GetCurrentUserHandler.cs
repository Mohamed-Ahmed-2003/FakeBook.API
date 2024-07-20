using AutoMapper;
using Fakebook.Application.CQRS.Account;
using Fakebook.Application.CQRS.Account.Dtos;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Identity.Queries;

public class GetCurrentUserHandler(DataContext ctx, UserManager<IdentityUser> userManager, IMapper mapper)
        : IRequestHandler<GetCurrentUser, Response<IdentityUserProfileDto>>
{
    private readonly DataContext _ctx = ctx;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<IdentityUserProfileDto>> Handle(GetCurrentUser request, 
        CancellationToken cancellationToken)
    {
        var result = new Response<IdentityUserProfileDto> ();
        var profile = await _ctx.UserProfiles
            .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken);

        if (profile is null)
        {
            result.AddError(Generics.Enums.StatusCode.ProfileNotFound, AccountErrorMessages.AccountNotFound);
            return result;
        }
        var identity = await _userManager.FindByIdAsync(profile.IdentityId);

        result.Payload = _mapper.Map<IdentityUserProfileDto>(profile);
        result.Payload.UserName = identity.UserName;

        return result;
    }
}