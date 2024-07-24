using FakeBook.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FakeBook.API.RealTime
{
      [Authorize]
    public class OnlineHub (OnlineTracker tracker) : Hub {
        private readonly OnlineTracker tracker = tracker;

        public override async Task OnConnectedAsync()
        {
            if (Context.User is null)
                throw new HubException("Cannot get current user claim");

            var userProfileId = Context.User.GetUserProfileId();

            var isOnline = await tracker
                .UserConnected(userProfileId, Context.ConnectionId);

            if (isOnline)
            {
                var friends = await tracker.GetFriendsConnections(userProfileId);
                foreach (var conn in friends)
                    await Clients.Client(conn).SendAsync("UserIsOnline", userProfileId);
            }

            var currentUsers = await tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User is null)
                throw new HubException("Cannot get current user claim");

            var userProfileId = Context.User.GetUserProfileId();

            var isOffline = await tracker
                .UserDisconnected(userProfileId, Context.ConnectionId);

            if (isOffline)
            {
                var friends = await tracker.GetFriendsConnections(userProfileId);

                foreach (var conn in friends)
                    await Clients.Client(conn).SendAsync("UserIsOffline", userProfileId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
