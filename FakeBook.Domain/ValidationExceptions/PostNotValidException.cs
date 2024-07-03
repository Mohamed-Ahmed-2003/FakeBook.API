
namespace FakeBook.Domain.ValidationExceptions
{
    public class PostNotValidException : NotValidException
    {
        public PostNotValidException() { }
        public PostNotValidException(string mesg) : base(mesg) { }
        public PostNotValidException(string mesg, Exception inner) : base(mesg, inner) { }
    }
}
