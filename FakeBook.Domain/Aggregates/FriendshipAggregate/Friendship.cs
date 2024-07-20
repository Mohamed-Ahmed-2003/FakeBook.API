
using FakeBook.Domain.Aggregates.UserProfileAggregate;

namespace FakeBook.Domain.Aggregates.FriendshipAggregate;
public class Friendship
{
    internal Friendship() { }
    public Guid FriendshipId { get; internal set; }
    public Guid? FirstFriendUserProfileId { get; internal set; }
    public UserProfile? FirstFriend { get; internal set; }
    public Guid? SecondFriendUserProfileId { get; internal set; }
    public UserProfile? SecondFriend { get; internal set; }
    public DateTime DateEstablished { get; internal set; }
    public FriendshipStatus FriendshipStatus { get; internal set; }

    public void Unfriend()
    {
        FriendshipStatus = FriendshipStatus.Inactive;
    }
}