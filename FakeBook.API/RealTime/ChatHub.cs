namespace FakeBook.API.RealTime
{
    using global::FakeBook.API.Extensions;
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace FakeBook.API.RealTime
    {
        public class ChatHub(OnlineTracker tracker) : Hub
        {
            private readonly OnlineTracker _tracker = tracker;

            public override async Task OnConnectedAsync()
            {
                if (Context.User is null)
                    throw new HubException("Cannot get current user claim");

                var userProfileId = Context.User.GetUserProfileId();

                // Link the user to their chat rooms
                var chatRooms = await GetUserChatRooms(userProfileId);
                foreach (var roomId in chatRooms)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                }

                //await Clients.Caller.SendAsync("ChatRooms", chatRooms);

                await base.OnConnectedAsync();
            }

            public override async Task OnDisconnectedAsync(Exception exception)
            {
                if (Context.User is null)
                    throw new HubException("Cannot get current user claim");

                var userProfileId = Context.User.GetUserProfileId();

                // Remove the user from their chat rooms
                var chatRooms = await GetUserChatRooms(userProfileId);
                foreach (var roomId in chatRooms)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
                }

                await base.OnDisconnectedAsync(exception);
            }

            private async Task<List<Guid>> GetUserChatRooms(Guid userProfileId)
            {
                return await _tracker.GetUserChatRooms(userProfileId);

           }

           
        }
    }


}
