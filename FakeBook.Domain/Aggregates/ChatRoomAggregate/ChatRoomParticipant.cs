namespace FakeBook.Domain.Aggregates.ChatRoomAggregate
{
    public class ChatRoomParticipant
    {
        public Guid Id { get; private set; }
        public Guid ChatRoomId { get; private set; }
        public Guid UserProfileId { get; private set; }
        

        private ChatRoomParticipant() { }

        public static ChatRoomParticipant CreateParticipant(Guid chatRoomId, Guid userProfileId)
        {
            return new ChatRoomParticipant
            {
                Id = Guid.NewGuid(),
                ChatRoomId = chatRoomId,
                UserProfileId = userProfileId
            };
        }
    }
}
