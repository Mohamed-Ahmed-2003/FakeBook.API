using MediatR;
using Fakebook.DAL;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Microsoft.EntityFrameworkCore;
namespace Fakebook.Application.Friendships.Commands;

public class RejectFriendCmd : IRequest<Response<Unit>>
{
    public Guid FriendRequestId { get; set; }
    public Guid ActionPerformedById { get; set; }
}

public class RejectFriendRequestHandler : IRequestHandler<RejectFriendCmd, Response<Unit>>
{
    private readonly DataContext _ctx;
    private readonly Response<Unit> _result = new();

    public RejectFriendRequestHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Response<Unit>> Handle(RejectFriendCmd request, 
        CancellationToken cancellationToken)
    {
        var friendRequest = await _ctx.FriendRequests
            .FirstOrDefaultAsync(fr 
                => fr.FriendRequestId == request.FriendRequestId && 
                   fr.ReceiverUserProfileId == request.ActionPerformedById, cancellationToken);

        if (friendRequest is null)
        {
            _result.AddError(StatusCodes.FriendRequestRejectNotPossible, 
                "Not possible to reject friend request");
            return _result;
        }

        friendRequest.RejectFriendRequest();
        
        _ctx.FriendRequests.Update(friendRequest);

        try
        {
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _result.AddError(StatusCodes.DatabaseOperationException, e.Message);
        }

        return _result;
    }
}