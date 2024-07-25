using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Generics.Interfaces;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using FakeBook.Domain.ValidationExceptions;
using MediatR;

namespace Fakebook.Application.CQRS.Chat.Commands
{
    #region CMD
    public class CreateChatRoomCmd : IRequest<Response<ChatRoom>>
    {
        public Guid FriendId { get; set; }
        public Guid UserProfileId { get; set; }
    }

    #endregion

    #region Handler

public class CreateChatRoomCmdHandler(IChatNotifier chatNotifier , DataContext context) : IRequestHandler<CreateChatRoomCmd, Response<ChatRoom>>
    {
        private readonly IChatNotifier _chatNotifier = chatNotifier;
        private readonly DataContext _context = context;

        public async Task<Response<ChatRoom>> Handle(CreateChatRoomCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<ChatRoom>();

            try
            {
                // Check if the user is friends
                bool isFriend = _context.Friendships.Any(f =>
                    (f.FirstFriendUserProfileId == request.FriendId || f.SecondFriendUserProfileId == request.FriendId)
                    && (f.FirstFriendUserProfileId == request.UserProfileId || f.SecondFriendUserProfileId == request.UserProfileId));

                if (!isFriend)
                {
                    response.AddError(StatusCodes.ChatRoomCreationFailed, ChatErrorMessages.UserNotFriends);
                    return response;
                }

                // Create the chat room
                var chatRoom = ChatRoom.CreateChatRoom(ChatRoomType.OneOnOne);
                chatRoom.AddOneOnOne(request.FriendId, request.UserProfileId);


                    // Add chat room to the context and save
                    _context.ChatRooms.Add(chatRoom);
                    await _context.SaveChangesAsync(cancellationToken);
                    
                   // Notify clients
                    await _chatNotifier.NotifyChatRoomCreated(chatRoom);
            

                // Set response payload
                response.Payload = chatRoom;
            }catch (ChatRoomNotValidException ex)
            {
                foreach (var err in ex.ValidationErrors)
                {
                    response.AddError(StatusCodes.ValidationError, err);
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                response.AddError(StatusCodes.UnknownError, ex.Message);
            }

            return response;
        }

    }

    #endregion

}
