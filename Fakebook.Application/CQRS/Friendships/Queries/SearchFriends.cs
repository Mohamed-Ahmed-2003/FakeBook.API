using MediatR;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using Microsoft.EntityFrameworkCore;
using Fakebook.Application.CQRS.Friendships.Dtos;
using FakeBook.Domain.Aggregates.UserProfileAggregate;

namespace Fakebook.Application.Friendships.Queries;

public class SearchFriends: IRequest<Response<List<FriendDto>>>
{
    public required Guid UserProfileId { get; set; }
    public required string SearchTerm { get; set; }
}

public class SearchFriendsQueryHandler : IRequestHandler<SearchFriends, Response<List<FriendDto>>>
{
    private readonly DataContext _ctx;
    private readonly Response<List<FriendDto>> _result = new();

    public SearchFriendsQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Response<List<FriendDto>>> Handle(SearchFriends request, CancellationToken cancellationToken)
    {
        var friendships = await _ctx.Friendships
            .Where(f => f.FirstFriendUserProfileId == request.UserProfileId || f.SecondFriendUserProfileId == request.UserProfileId)
            .Select(f => new
            {
                DateEstablished = f.DateEstablished,
                Profile = f.FirstFriendUserProfileId == request.UserProfileId ? f.SecondFriend : f.FirstFriend
            })
            .ToListAsync(cancellationToken);

        var friends = friendships
            .Select(friend => new FriendDto
            {
                FriendshipStartedAt = friend.DateEstablished,
                FriendId = friend.Profile.UserProfileId,
                Name = friend.Profile.GeneralInfo.FirstName + " " + friend.Profile.GeneralInfo.LastName,
                Email = friend.Profile.GeneralInfo.EmailAddress
            })
            .Where(f => f.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) || f.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        _result.Payload = friends;
        return _result;
    }



}
