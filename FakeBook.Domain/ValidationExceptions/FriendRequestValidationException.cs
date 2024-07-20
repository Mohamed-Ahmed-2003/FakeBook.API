namespace FakeBook.Domain.ValidationExceptions;

public class FriendRequestValidationException : NotValidException
{
    internal FriendRequestValidationException() {}
    internal FriendRequestValidationException(string message) : base(message) {}
    internal FriendRequestValidationException(string message, Exception inner) : base(message, inner) {}
}