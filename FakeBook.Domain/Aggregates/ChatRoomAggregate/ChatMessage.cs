using FakeBook.Domain.Constants;
using FakeBook.Domain.ValidationExceptions;

namespace FakeBook.Domain.Aggregates.ChatRoomAggregate
{
  
    public class ChatMessage
    {
        public Guid Id { get; private set; }
        public Guid ChatRoomId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public string Content { get; private set; }
        public DateTime SentAt { get; private set; }
        public DateTime LastEdited { get; private set; }

        private ChatMessage() { }

        public static ChatMessage CreateMessage(Guid chatRoomId, Guid userProfileId, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                var ex = new ChatMessageNotValidException(Helper.ExceptionsMessages.ChatMessageContentNotValidException);
                ex.ValidationErrors.Add("Message content cannot be empty.");
                throw ex;
            }
            var dateNow = DateTime.UtcNow;
            return new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatRoomId = chatRoomId,
                UserProfileId = userProfileId,
                Content = content,
                SentAt = dateNow,
                LastEdited = dateNow,
            };
        }
        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrEmpty(newContent))
            {
                var ex = new ChatMessageNotValidException(Helper.ExceptionsMessages.ChatMessageContentNotValidException);
                ex.ValidationErrors.Add("Message content cannot be empty.");
                throw ex;
            }

            Content = newContent;
            LastEdited = DateTime.UtcNow;
        }
    }
}
