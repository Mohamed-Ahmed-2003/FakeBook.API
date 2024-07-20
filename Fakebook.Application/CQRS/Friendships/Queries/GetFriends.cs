using MediatR;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using Microsoft.EntityFrameworkCore;
using Fakebook.Application.CQRS.Friendships.Dtos;

namespace Fakebook.Application.Friendships.Queries;

public class GetFriends : IRequest<Response<List<FriendDto>>>
{
    public Guid UserId { get; set; }
}

public class GetFriendsHandler : IRequestHandler<GetFriends, Response<List<FriendDto>>>
{
    private readonly DataContext _ctx;
    private readonly Response<List<FriendDto>> _result = new();

    public GetFriendsHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Response<List<FriendDto>>> Handle(GetFriends request, CancellationToken cancellationToken)
    {
        var friends = await _ctx.Friendships
               .Where(f => f.FirstFriendUserProfileId == request.UserId || f.SecondFriendUserProfileId == request.UserId)
               .Select(f => f.FirstFriendUserProfileId == request.UserId ? f.SecondFriend : f.FirstFriend)
               .Select(friend => new FriendDto
               {
                   FriendId = friend.UserProfileId,
                   Name = friend.GeneralInfo.FirstName + " " + friend.GeneralInfo.LastName,
                   Email = friend.GeneralInfo.EmailAddress
               }).ToListAsync(cancellationToken);


        _result.Payload = friends;
        return _result;
    }

}
