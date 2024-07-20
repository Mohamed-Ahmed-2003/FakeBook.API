using MediatR;
using Fakebook.DAL;
using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.FriendshipAggregate;
using FakeBook.Domain.ValidationExceptions;
using Fakebook.Application.Generics.Enums;
namespace Fakebook.Application.Friendships.Commands;

public class CreateFriendCmd : IRequest<Response<Unit>>
{
    public Guid RequesterId { get; set; }
    public Guid ReceiverId { get; set; }
}

public class CreateFriendRequestHandler : IRequestHandler<CreateFriendCmd, Response<Unit>>
{
    private readonly DataContext _ctx;
    private readonly Response<Unit> _result = new();

    public CreateFriendRequestHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Response<Unit>> Handle(CreateFriendCmd request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var friendRequest = FriendRequest
                .CreateFriendRequest(Guid.NewGuid(), request.RequesterId, request.ReceiverId, DateTime.UtcNow);
            _ctx.FriendRequests.Add(friendRequest);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch (FriendRequestValidationException ex)
        {
            _result.AddError(StatusCode.FriendRequestValidationError, ex.Message);
        }
        catch (Exception e)
        {
            _result.AddError(StatusCode.DatabaseOperationException, e.Message);
        }
        
        return _result;
    }
}