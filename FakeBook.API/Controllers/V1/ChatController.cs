using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using MediatR;
using Fakebook.Application.CQRS.Chat.Commands;
using Asp.Versioning;
using FakeBook.API.Filters;
using FakeBook.API.Controllers.V1;
using FakeBook.API.Extensions;
using FakeBook.API.Contracts.Chat.Responses;
using FakeBook.API.Contracts.Chat.Requests;
using Fakebook.Application.CQRS.Chat.Queries;

namespace FakeBook.API.Controllers
    {
        [ApiVersion("1.0")]
        [Route(ApiRoutes.BaseRoute)]
        [ApiController]
        [Authorize]
        public class ChatController(IMapper mapper, IMediator mediator) : BaseController
        {
            private readonly IMapper _mapper = mapper;
            private readonly IMediator _mediator = mediator;

            [HttpPost(ApiRoutes.Chat.Rooms)]
            [ValidateGuid("friendId")]
            public async Task<IActionResult> CreateChatRoom(Guid friendId)
            {
                var userProfileId = HttpContext.User.GetUserProfileId();

                var cmd = new CreateChatRoomCmd
                {
                    UserProfileId = userProfileId,
                    FriendId = friendId
                };

                var cmdResult = await _mediator.Send(cmd);
                if (!cmdResult.Success)
                {
                    return HandleErrorResponse(cmdResult.Errors);
                }

                var chatRoomResponse = _mapper.Map<AbstractChatRoom>(cmdResult.Payload);
                return CreatedAtAction(nameof(GetChatRooms), new { id = chatRoomResponse.ChatRoomId }, chatRoomResponse);
            }

            [HttpGet(ApiRoutes.Chat.Rooms)]
            public async Task<IActionResult> GetChatRooms()
            {
            var userProfileId = HttpContext.User.GetUserProfileId();
            var query = new GetChatRoomsQuery { UserProfileId = userProfileId };

            var queryResult = await _mediator.Send(query);
            if (!queryResult.Success)
            {
                return HandleErrorResponse(queryResult.Errors);
            }

            var chatRoomsResponse = _mapper.Map<List<AbstractChatRoom>>(queryResult.Payload);
            return Ok(chatRoomsResponse);

            }

            [HttpGet(ApiRoutes.Chat.Messages.GetMessages)]
            [ValidateGuid("roomId")]
            public async Task<IActionResult> GetChatMessages(Guid roomId)
            {
            var query = new GetChatMessages { RoomId = roomId, UserProfileId = HttpContext.User.GetUserProfileId() };

            var queryResult = await _mediator.Send(query);
            if (!queryResult.Success)
            {
                return HandleErrorResponse(queryResult.Errors);
            }

            var messagesResponse = _mapper.Map<List<AbstractChatMessage>>(queryResult.Payload);
            return Ok(messagesResponse);
        }

            [HttpPost(ApiRoutes.Chat.Messages.SendMessage)]
            [ValidateGuid("roomId")]
            public async Task<IActionResult> SendMessage(Guid roomId, [FromBody] SendChatMessage request)
            {
                var cmd = new SendChatMessageCmd
                {
                    RoomId = roomId,
                    UserProfileId = HttpContext.User.GetUserProfileId(),
                    Content = request.Content
                };

                var cmdResult = await _mediator.Send(cmd);
                if (!cmdResult.Success)
                {
                    return HandleErrorResponse(cmdResult.Errors);
                }

                return Ok();
            }

            [HttpPut(ApiRoutes.Chat.Messages.UpdateMessage)]
            [ValidateGuid("roomId", "messageId")]
            public async Task<IActionResult> UpdateMessage(Guid roomId, Guid messageId, [FromBody] UpdateChatMessage request)
            {
            var cmd = new UpdateChatMessageCmd
            {
                RoomId = roomId,
                MessageId = messageId,
                NewContent = request.NewContent,
            };

            var cmdResult = await _mediator.Send(cmd);
            if (!cmdResult.Success)
            {
                return HandleErrorResponse(cmdResult.Errors);
            }

            return NoContent();
        }

            [HttpDelete(ApiRoutes.Chat.Messages.DeleteMessage)]
            [ValidateGuid("roomId", "messageId")]
            public async Task<IActionResult> DeleteMessage(Guid roomId, Guid messageId)
            {
            var cmd = new DeleteChatMessageCmd
            {
                RoomId = roomId,
                MessageId = messageId,
                UserProfileId = HttpContext.User.GetUserProfileId()
            };

            var cmdResult = await _mediator.Send(cmd);
            if (!cmdResult.Success)
            {
                return HandleErrorResponse(cmdResult.Errors);
            }

            return NoContent();
            }

        [HttpGet(ApiRoutes.Chat.Messages.SearchMessage)]
        public async Task<IActionResult> SearchMessages([FromQuery] SearchMessages queryParams)
        {
            var query = new SearchMessagesCmd
            {
                SearchTerm = queryParams.SearchTerm,
                StartDate = queryParams.StartDate,
                EndDate = queryParams.EndDate,
                UserProfileId = HttpContext.User.GetUserProfileId()
            };

            var queryResult = await _mediator.Send(query);
            if (!queryResult.Success)
            {
                return HandleErrorResponse(queryResult.Errors);
            }

            var messagesResponse = _mapper.Map<List<AbstractChatMessage>>(queryResult.Payload);
            return Ok(messagesResponse);
        }


    }

}