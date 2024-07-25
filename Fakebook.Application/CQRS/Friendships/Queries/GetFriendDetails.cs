using MediatR;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using Microsoft.EntityFrameworkCore;
using Fakebook.Application.CQRS.Friendships.Dtos;
using Fakebook.Application.Generics.Enums;

namespace Fakebook.Application.Friendships.Queries;

public class GetFriendDetails : IRequest<Response<FriendDto>>
{
    public Guid FriendId { get; set; }
}

public class GetFriendDetailsHandler(DataContext ctx) : IRequestHandler<GetFriendDetails, Response<FriendDto>>
{
    private readonly DataContext _ctx = ctx;
    private readonly Response<FriendDto> _result = new();

    public async Task<Response<FriendDto>> Handle(GetFriendDetails request, CancellationToken cancellationToken)
    {
        var friend = await _ctx.UserProfiles
            .Where(u => u.UserProfileId == request.FriendId)
            .Select(u => new FriendDto
            {
                FriendId = u.UserProfileId,
                Name = string.Concat(u.GeneralInfo.FirstName," ", u.GeneralInfo.LastName),
                Email = u.GeneralInfo.EmailAddress,
                // Other details you want to include
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (friend == null)
        {
            _result.AddError(StatusCodes.FriendRequestRejectNotPossible, "Friend not found");
            return _result;
        }

        _result.Payload = friend;
        return _result;
    }
}
