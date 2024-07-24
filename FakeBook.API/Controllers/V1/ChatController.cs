using Asp.Versioning;
using AutoMapper;
using Fakebook.Application.Generics.Interfaces;
using FakeBook.API.Contracts.Chat.Requests;
using FakeBook.API.Filters;
using FakeBook.API.RealTime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Fakebook.Application.CQRS.Chat.Commands;
    using Fakebook.Application.CQRS.Chat.Queries;
    using global::FakeBook.API.Extensions;
    using global::FakeBook.API.Contracts.Chat.Responses;

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
                //var userProfileId = HttpContext.User.GetUserProfileId();
                //var query = new GetChatRoomsQuery { UserProfileId = userProfileId };

                //var queryResult = await _mediator.Send(query);
                //if (!queryResult.Success)
                //{
                //    return HandleErrorResponse(queryResult.Errors);
                //}

                //var chatRoomsResponse = _mapper.Map<List<AbstractChatRoom>>(queryResult.Payload);
                //return Ok(chatRoomsResponse);
                throw new NotImplementedException();

            }

            [HttpGet(ApiRoutes.Chat.Messages.GetMessages)]
            [ValidateGuid("roomId")]
            public async Task<IActionResult> GetChatMessages(Guid roomId)
            {
                //var query = new GetChatMessages { RoomId = roomId,UserProfileId = HttpContext.User.GetUserProfileId() };

                //var queryResult = await _mediator.Send(query);
                //if (!queryResult.Success)
                //{
                //    return HandleErrorResponse(queryResult.Errors);
                //}

                //var messagesResponse = _mapper.Map<List<AbstractChatMessage>>(queryResult.Payload);
                //return Ok(messagesResponse);

                throw new NotImplementedException();

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
                //var cmd = new UpdateChatMessageCmd
                //{
                //    RoomId = roomId,
                //    MessageId = messageId,
                //    NewContent = request.NewContent,
                //    UserProfileId = HttpContext.User.GetUserProfileId()
                //};

                //var cmdResult = await _mediator.Send(cmd);
                //if (!cmdResult.Success)
                //{
                //    return HandleErrorResponse(cmdResult.Errors);
                //}

                //return NoContent();
                throw new NotImplementedException();
            }

            [HttpDelete(ApiRoutes.Chat.Messages.DeleteMessage)]
            [ValidateGuid("roomId", "messageId")]
            public async Task<IActionResult> DeleteMessage(Guid roomId, Guid messageId)
            {
                //var cmd = new DeleteChatMessageCmd
                //{
                //    RoomId = roomId,
                //    MessageId = messageId,
                //    UserProfileId = HttpContext.User.GetUserProfileId()
                //};

                //var cmdResult = await _mediator.Send(cmd);
                //if (!cmdResult.Success)
                //{
                //    return HandleErrorResponse(cmdResult.Errors);
                //}

                //return NoContent();
                throw new NotImplementedException();

            }
        }
    }




}
