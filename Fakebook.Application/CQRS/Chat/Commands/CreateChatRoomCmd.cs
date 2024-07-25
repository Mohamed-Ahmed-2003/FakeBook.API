using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Generics.Interfaces;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Chat.Commands
{
    #region CMD
    public class CreateChatRoomCmd : IRequest<Response<ChatRoom>>
    {
        public Guid FriendId { get; set; }
        public Guid UserProfileId { get; set; }
        public ChatRoomType RoomType{ get; set; }
    }

    #endregion

    #region Handler

    public class CreateChatRoomCmdHandler : IRequestHandler<CreateChatRoomCmd, Response<ChatRoom>>
    {
        private readonly IChatNotifier _chatNotifier;
        private readonly DataContext _context;

        public CreateChatRoomCmdHandler(IChatNotifier chatNotifier, DataContext context)
        {
            _chatNotifier = chatNotifier;
            _context = context;
        }

        public async Task<Response<ChatRoom>> Handle(CreateChatRoomCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<ChatRoom>();

            try
            {
                // Check if the chat room already exists for OneOnOne type
                if (request.RoomType == ChatRoomType.OneOnOne)
                {
                    bool isFriend = await _context.Friendships
                        .AnyAsync(f =>
                            (f.FirstFriendUserProfileId == request.FriendId && f.SecondFriendUserProfileId == request.UserProfileId) ||
                            (f.FirstFriendUserProfileId == request.UserProfileId && f.SecondFriendUserProfileId == request.FriendId));

                    if (!isFriend)
                    {
                        response.AddError(StatusCodes.ChatRoomCreationFailed, ChatErrorMessages.UserNotFriends);
                        return response;
                    }

                    bool roomExists = _context.ChatRooms
                        .Any(cr => cr.RoomType == ChatRoomType.OneOnOne
                            && cr.Participants.Any
                            (p =>p.UserProfileId == request.UserProfileId ) 
                            && 
                            cr.Participants.Any (p =>p.UserProfileId == request.FriendId ) 
                        );

                    if (roomExists)
                    {
                        response.AddError(StatusCodes.ChatRoomAlreadyExists, ChatErrorMessages.ChatRoomAlreadyExists);
                        return response;
                    }
                }

                // Create the chat room
                var chatRoom = ChatRoom.CreateChatRoom(request.RoomType);

                if (request.RoomType == ChatRoomType.OneOnOne)
                {
                    chatRoom.AddOneOnOne(request.FriendId, request.UserProfileId);
                }

                // Add chat room to the context and save
                _context.ChatRooms.Add(chatRoom);
                await _context.SaveChangesAsync(cancellationToken);

                // Notify clients
                await _chatNotifier.NotifyChatRoomCreated(chatRoom);

                // Set response payload
                response.Payload = chatRoom;
            }
            catch (ChatRoomNotValidException ex)
            {
                foreach (var err in ex.ValidationErrors)
                {
                    response.AddError(StatusCodes.ValidationError, err);
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                response.AddError(StatusCodes.UnknownError, "An unexpected error occurred: " + ex.Message);
            }

            return response;
        }
    }


    #endregion

}
