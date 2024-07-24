using Fakebook.Application.CQRS.Chat.Queries;
using Fakebook.Application.Friendships.Queries;
using MediatR;
using System.Text;

namespace FakeBook.API.RealTime
{
    public class OnlineTracker (IMediator mediator)
    { 
        // each user has # of clients
        private static readonly Dictionary<Guid, List<string>> OnlineUsers = [];
        private readonly IMediator _mediator = mediator;

        public Task<bool> UserConnected(Guid userId, string connectionId)
        {
            var isOnline = false;
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(userId))
                    OnlineUsers[userId].Add(connectionId);
                else
                {
                    OnlineUsers.Add(userId, [connectionId]);
                    isOnline = true;
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(Guid userId, string connectionId)
        {
            var isOffline = false;
            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(userId))
                    return Task.FromResult(isOffline);

                OnlineUsers[userId].Remove(connectionId);

                if (OnlineUsers[userId].Count == 0)
                {
                    OnlineUsers.Remove(userId);
                    isOffline = true;
                }
            }

            return Task.FromResult(isOffline);
        }

        public Task<Guid[]> GetOnlineUsers()
        {
            Guid[] onlineUsers;

            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers
                    .OrderBy(x => x.Key)
                    .Select(x => x.Key)
                    .ToArray();
            }

            return Task.FromResult(onlineUsers);
        }

        public static Task<List<string>> GetConnectionsForUser(Guid userId)
        {
            List<string> connectionIds;
            if (OnlineUsers.TryGetValue(userId, out var connections))
            {
                lock (connections)
                {
                    connectionIds = [.. connections];
                }
            }
            else
            {
                connectionIds = [];
            }

            return Task.FromResult(connectionIds);
        }
        public  async Task<List<string>> GetFriendsConnections (Guid userId)
        {
            var query = await _mediator.Send(new GetFriends { UserId = userId });
            var connections = new List<string>();
            foreach (var friend in query.Payload) {
            
            connections.AddRange(await GetConnectionsForUser(friend.FriendId));
            }
            return connections;
        }  
        
        public  async Task<List<Guid>> GetUserChatRooms (Guid userId)
        {
            var query = await _mediator.Send(new GetChatRoomsQuery { UserProfileId = userId });


            var RoomsIds  = query.Payload.Select(r=>r.Id).ToList();
          
            return RoomsIds;
        }

    }
}
