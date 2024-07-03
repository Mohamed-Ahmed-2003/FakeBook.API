

namespace FakeBook.Domain.ValidationExceptions
{
    public class NotValidException : Exception
    {
        public List<string> ValidationErrors { get; } = new List<string>();
        public NotValidException() { }
        public NotValidException(string mesg) : base(mesg) { }
        public NotValidException(string mesg,Exception inner) : base(mesg,inner) { }
    }
}
