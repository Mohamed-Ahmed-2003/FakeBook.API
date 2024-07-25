
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using System.Linq.Expressions;

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
    //public Expression AreFriends(Guid friend1 , Guid friend2)
    //{
    //    return (FirstFriendUserProfileId == friend1 || SecondFriendUserProfileId == friend1  ) 
    //        && ( FirstFriendUserProfileId == friend2|| SecondFriendUserProfileId == friend2 );
    //}

}