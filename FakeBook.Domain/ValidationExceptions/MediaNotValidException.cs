namespace FakeBook.Domain.ValidationExceptions
{
    public class MediaNotValidException: NotValidException
    {
        public MediaNotValidException() { }
        public MediaNotValidException(string mesg) : base(mesg) { }
        public MediaNotValidException(string mesg, Exception inner) : base(mesg, inner) { }
    }
}
