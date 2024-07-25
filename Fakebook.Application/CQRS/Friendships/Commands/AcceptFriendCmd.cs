
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Friendships.Commands;

public class AcceptFriendCmd : IRequest<Response<Unit>>
{
    public Guid FriendRequestId { get; set; }
    public Guid ActionPerformedById { get; set; }
}

public class AcceptFriendRequestHandler : IRequestHandler<AcceptFriendCmd, Response<Unit>>
{
    private readonly DataContext _ctx;

    public AcceptFriendRequestHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    private readonly Response<Unit> _result = new();
    public async Task<Response<Unit>> Handle(AcceptFriendCmd request, 
        CancellationToken cancellationToken)
    {
        var friendRequest = await _ctx.FriendRequests
            .FirstOrDefaultAsync(fr 
                => fr.FriendRequestId == request.FriendRequestId && 
                   fr.ReceiverUserProfileId == request.ActionPerformedById, cancellationToken);

        if (friendRequest is null)
        {
            _result.AddError(StatusCodes.FriendRequestAcceptNotPossible, 
                "Not possible to accept friend request");
            return _result;
        }

        var friendship = friendRequest.AcceptFriendRequest(Guid.NewGuid());

        await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
        _ctx.FriendRequests.Update(friendRequest);
        _ctx.Friendships.Add(friendship!);

        try
        {
            await _ctx.SaveChangesAsync(cancellationToken);
            await _ctx.Database.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _ctx.Database.RollbackTransactionAsync(cancellationToken);
            _result.AddError(StatusCodes.DatabaseOperationException, e.Message);
        }

        return _result;
    }
}