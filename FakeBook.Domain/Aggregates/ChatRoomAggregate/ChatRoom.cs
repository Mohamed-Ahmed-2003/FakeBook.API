using FakeBook.Domain.Constants;
using FakeBook.Domain.ValidationExceptions;
using FakeBook.Domain.Validators.ChatValidators;

namespace FakeBook.Domain.Aggregates.ChatRoomAggregate
{
        public enum ChatRoomType
        {
            OneOnOne,
            Group
        }
    

    public class ChatRoom
    {
        private readonly List<ChatRoomParticipant> _participants = new List<ChatRoomParticipant>();
        private readonly List<ChatMessage> _messages = new List<ChatMessage>();

        private ChatRoom() { }

        #region Properties
        public Guid Id { get; private set; }
        public ChatRoomType RoomType { get; private set; }
        public DateTime CreatedDate { get; private set; }
        #endregion

        #region Nav Props
        public IEnumerable<ChatRoomParticipant> Participants => _participants;
        public IEnumerable<ChatMessage> Messages => _messages;
        #endregion

        #region FM
        public static ChatRoom CreateChatRoom(ChatRoomType roomType)
        {
            var validator = new ChatRoomValidator();
            var chatRoom = new ChatRoom
            {
                Id = Guid.NewGuid(),
                RoomType = roomType,
                CreatedDate = DateTime.UtcNow
            };

            var res = validator.Validate(chatRoom);
            if (!res.IsValid)
            {
                var ex = new ChatRoomNotValidException(Helper.ExceptionsMessages.ChatRoomNotValidException);
                foreach (var error in res.Errors)
                {
                    ex.ValidationErrors.Add(error.ErrorMessage);
                }
                throw ex;
            }

            return chatRoom;
        }
        #endregion

        public void AddParticipant(ChatRoomParticipant participant)
        {
            _participants.Add(participant);
        } 
        
        public void AddOneOnOne (Guid friendId1 , Guid friendId2)
        {
            AddParticipant(ChatRoomParticipant.CreateParticipant(Id, friendId1));
            AddParticipant(ChatRoomParticipant.CreateParticipant(Id, friendId2));

        }

        public void RemoveParticipant(ChatRoomParticipant participant)
        {
            _participants.Remove(participant);
        }

        public void SendMessage(ChatMessage message)
        {
            _messages.Add(message);
        }

        public void RemoveMessage(ChatMessage message)
        {
            _messages.Remove(message);
        } 
              
    }
}
