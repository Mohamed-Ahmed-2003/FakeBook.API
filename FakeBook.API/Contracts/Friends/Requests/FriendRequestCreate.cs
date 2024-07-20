namespace FakeBook.API.Contracts.Friends.Requests;

public class FriendRequestCreate
{
    public Guid RequesterId { get; set; }
    
    public Guid ReceiverId { get; set; }
}