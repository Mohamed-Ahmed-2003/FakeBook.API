using AutoMapper;
using Fakebook.Application.Friendships.Commands;
using Fakebook.Application.Friendships.Queries;
using FakeBook.API;
using FakeBook.API.Contracts.Friends.Requests;
using FakeBook.API.Controllers.V1;
using FakeBook.API.Extensions;
using FakeBook.API.Filters;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.Api.Controllers.V1;

[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FriendsController(IMapper mapper, IMediator mediator) : BaseController
{
    private readonly IMapper _mapper = mapper;
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Route(ApiRoutes.Friendships.FriendRequestCreate)]
    [ValidateModel]
    public async Task<IActionResult> SendFriendRequest(FriendRequestCreate friendRequestCreate,
        CancellationToken token)
    {
        var command = new CreateFriendCmd
        {
            RequesterId = friendRequestCreate.RequesterId,
            ReceiverId = friendRequestCreate.ReceiverId
        };
        var result = await _mediator.Send(command, token);
        if (!result.Success) HandleErrorResponse(result.Errors);
        return NoContent();
    }

    [HttpPost]
    [Route(ApiRoutes.Friendships.FriendRequestAccept)]
    [ValidateGuid("friendRequestId")]
    public async Task<IActionResult> AcceptFriendRequest(Guid friendRequestId, CancellationToken token)
    {
        var actionPerformedBy = HttpContext.GetUserProfileId();
        var command = new AcceptFriendCmd
        {
            FriendRequestId = friendRequestId,
            ActionPerformedById = actionPerformedBy
        };
        var result = await _mediator.Send(command,token);
        if (!result.Success) return HandleErrorResponse(result.Errors);
        return NoContent();
    }

    [HttpPost]
    [Route(ApiRoutes.Friendships.FriendRequestReject)]
    [ValidateGuid("friendRequestId")]
    public async Task<IActionResult> RejectFriendRequest(Guid friendRequestId, CancellationToken token)
    {
        var actionPerformedBy = HttpContext.GetUserProfileId();
        var command = new RejectFriendCmd
        {
            FriendRequestId = friendRequestId,
            ActionPerformedById = actionPerformedBy
        };
        var result = await _mediator.Send(command,token);
        if (!result.Success) return HandleErrorResponse(result.Errors);
        return NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Friendships.GetFriendRequests)]
    public async Task<IActionResult> GetFriendRequests(CancellationToken token)
    {
        var userId = HttpContext.GetUserProfileId();
        var query = new GetFriendRequests
        {
            UserId = userId
        };
        var result = await _mediator.Send(query, token);
        if (!result.Success) return HandleErrorResponse(result.Errors);
        return Ok(result.Payload); // Assume result.Data contains the list of friend requests
    }

    [HttpGet]
    [Route(ApiRoutes.Friendships.ListFriends)]
    public async Task<IActionResult> ListFriends(CancellationToken token)
    {
        var userId = HttpContext.GetUserProfileId();
        var query = new GetFriends
        {
            UserId = userId
        };
        var result = await _mediator.Send(query, token);
        if (!result.Success) return HandleErrorResponse(result.Errors);
        return Ok(result.Payload); // Assume result.Data contains the list of friends
    }

    [HttpPost]
    [Route(ApiRoutes.Friendships.RemoveFriend)]
    [ValidateGuid("friendId")]
    public async Task<IActionResult> RemoveFriend(Guid friendId, CancellationToken token)
    {
        var userId = HttpContext.GetUserProfileId();
        var command = new RemoveFriendCmd
        {
            UserId = userId,
            FriendId = friendId
        };
        var result = await _mediator.Send(command, token);
        if (!result.Success) return HandleErrorResponse(result.Errors);
        return NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Friendships.GetFriendDetails)]
    [ValidateGuid("friendId")]
    public async Task<IActionResult> GetFriendDetails(Guid friendId, CancellationToken token)
    {
        var query = new GetFriendDetails
        {
            FriendId = friendId
        };
        var result = await _mediator.Send(query, token);
        if (!result.Success) return HandleErrorResponse(result.Errors);
        return Ok(result.Payload); // Assume result.Data contains the friend's details
    }

    [HttpGet]
    [Route(ApiRoutes.Friendships.SearchFriends)]
    public async Task<IActionResult> SearchFriends([FromQuery] string searchTerm, CancellationToken token)
    {
        var query = new SearchFriends
        {
            UserProfileId = HttpContext.GetUserProfileId(),
            SearchTerm = searchTerm
        };
        var result = await _mediator.Send(query, token);
        if (!result.Success) return HandleErrorResponse(result.Errors);
        return Ok(result.Payload); // Assume result.Data contains the search results
    }
}