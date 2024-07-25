using FakeBook.Domain.Aggregates.ChatRoomAggregate;

namespace FakeBook.API.Contracts.Chat.Responses
{
    public class AbstractChatRoom
    {
        public Guid ChatRoomId { get;  set; }
        public required string Name { get;  set; }
        public ChatRoomType RoomType { get; private set; }
        public List<Guid> Participants { get; set; } = new List<Guid>();
        public List <AbstractChatMessage>? Messages { get; set; }

    }
}
