
namespace FakeBook.Domain.ValidationExceptions
{
    public class PostCommentNotValidException : NotValidException
    {
        public PostCommentNotValidException() { }
        public PostCommentNotValidException(string mesg) : base(mesg) { }
        public PostCommentNotValidException(string mesg, Exception inner) : base(mesg, inner) { }
    }
}
