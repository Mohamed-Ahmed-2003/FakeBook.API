using MediatR;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using Microsoft.EntityFrameworkCore;
using Fakebook.Application.CQRS.Friendships.Dtos;

namespace Fakebook.Application.Friendships.Queries;

public class GetFriendRequests : IRequest<Response<List<FriendRequestDto>>>
{
    public Guid UserId { get; set; }
}

public class GetFriendRequestsHandler : IRequestHandler<GetFriendRequests, Response<List<FriendRequestDto>>>
{
    private readonly DataContext _ctx;
    private readonly Response<List<FriendRequestDto>> _result = new();

    public GetFriendRequestsHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Response<List<FriendRequestDto>>> Handle(GetFriendRequests request, CancellationToken cancellationToken)
    {
        var friendRequests = await _ctx.FriendRequests
            .Where(fr => fr.ReceiverUserProfileId == request.UserId)
            .Select(fr => new FriendRequestDto
            {
                FriendRequestId = fr.FriendRequestId,
                RequesterId = (Guid) fr.RequesterUserProfileId,
                CreatedAt = fr.DateSent
            })
            .ToListAsync(cancellationToken);

        _result.Payload = friendRequests;
        return _result;
    }
}
