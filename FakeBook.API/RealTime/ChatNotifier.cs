using Fakebook.Application.Generics.Interfaces;
using FakeBook.API.RealTime.FakeBook.API.RealTime;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using Microsoft.AspNetCore.SignalR;

namespace FakeBook.API.RealTime
{
    public class ChatNotifier (IHubContext<ChatHub> hubContext) : IChatNotifier
    {
        private readonly IHubContext<ChatHub> _hubContext = hubContext;

        public async Task NotifyChatRoomCreated(ChatRoom chatRoom )
        {

            var chatRoomName = chatRoom.Id.ToString();
            foreach (var participant in chatRoom.Participants)
            {
                foreach (var connId in await OnlineTracker.GetConnectionsForUser(participant.UserProfileId))
                {
                await _hubContext.Groups.AddToGroupAsync(connId, chatRoomName);
                }
            }

            await _hubContext.Clients.Group(chatRoomName).SendAsync("ChatRoomCreated", chatRoom);

        }

        public async Task NotifyMessageDeleted(ChatMessage chatMessage)
        {
            await _hubContext.Clients.Group(chatMessage.ChatRoomId.ToString()).SendAsync("MessageDeleted", chatMessage);
        }

        public async Task NotifyMessageSent(ChatMessage chatMessage)
        {
         
            await _hubContext.Clients.Group(chatMessage.ChatRoomId.ToString()).SendAsync("RecieveMessage", chatMessage);

        }

        public async Task NotifyMessageUpdated(ChatMessage chatMessage)
        {
            await _hubContext.Clients.Group(chatMessage.ChatRoomId.ToString()).SendAsync("MessageUpdated", chatMessage);
        }
    }
}
