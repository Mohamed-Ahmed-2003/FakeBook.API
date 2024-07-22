

namespace FakeBook.Domain.ValidationExceptions
{
    public class ChatMessageNotValidException : NotValidException
    {
        public ChatMessageNotValidException() { }
        public ChatMessageNotValidException(string mesg) : base(mesg) { }
        public ChatMessageNotValidException(string mesg, Exception inner) : base(mesg, inner) { }
    }
}
