
namespace FakeBook.Domain.ValidationExceptions
{
    public class ChatRoomNotValidException : NotValidException
    {
        public ChatRoomNotValidException() { }
        public ChatRoomNotValidException(string mesg) : base(mesg) { }
        public ChatRoomNotValidException(string mesg, Exception inner) : base(mesg, inner) { }
    }
}
