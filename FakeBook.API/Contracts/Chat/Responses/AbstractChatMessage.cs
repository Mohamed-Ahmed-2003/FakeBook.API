namespace FakeBook.API.Contracts.Chat.Responses
{
    public class AbstractChatMessage
    {
        public Guid ChatMessageId { get;  set; }
        public Guid ChatRoomId { get;  set; }
        public Guid UserProfileId { get; private set; }
        public required string Content { get;  set; }
        public DateTime SentAt { get;  set; }
        public DateTime LastEdited { get;  set; }
        public bool IsRead { get; set; }

    }
}
