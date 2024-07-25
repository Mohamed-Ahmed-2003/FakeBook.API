using MediatR;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Friendships.Commands;

public class RemoveFriendCmd: IRequest<Response<Unit>>
{
    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
}

public class RemoveFriendCommandHandler(DataContext ctx) : IRequestHandler<RemoveFriendCmd, Response<Unit>>
{
    private readonly DataContext _ctx = ctx;
    private readonly Response<Unit> _result = new();

    public async Task<Response<Unit>> Handle(RemoveFriendCmd request, CancellationToken cancellationToken)
    {
        var friendship = await _ctx.Friendships
            .FirstOrDefaultAsync(f => (f.FirstFriendUserProfileId  == request.UserId && f.SecondFriendUserProfileId == request.FriendId) ||
                                       (f.FirstFriendUserProfileId == request.FriendId && f.SecondFriendUserProfileId == request.UserId), cancellationToken);

        if (friendship is null)
        {
            _result.AddError(StatusCodes.FriendRequestRejectNotPossible, "Friendship does not exist");
            return _result;
        }

        _ctx.Friendships.Remove(friendship);

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
