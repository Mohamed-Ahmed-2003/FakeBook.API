using FakeBook.Domain.Aggregates.UserProfileAggregate;
using FakeBook.Domain.Validators.FriendshipsValidators;

namespace FakeBook.Domain.Aggregates.FriendshipAggregate;

public class FriendRequest
{
    private FriendRequest() {}
    public Guid FriendRequestId { get; private set; }
    public Guid? RequesterUserProfileId { get; private set; }
    public UserProfile? Requester { get; private set; }
    public Guid? ReceiverUserProfileId { get; private set; }
    public UserProfile? Receiver { get; private set; }
    public DateTime DateSent { get; private set; }
    public DateTime DateResponded { get; private set; }
    public ResponseType Response { get; private set; }


    public static FriendRequest CreateFriendRequest(Guid friendRequestId, Guid requesterId, Guid receiverId,
        DateTime dateSent)
    {
        var friendRequest = new FriendRequest();
        friendRequest.FriendRequestId = friendRequestId;
        friendRequest.RequesterUserProfileId = requesterId;
        friendRequest.ReceiverUserProfileId = receiverId;
        friendRequest.DateSent = dateSent;
        friendRequest.Response = ResponseType.Pending;

        FriendshipAggregateValidator.ValidateFriendRequest(friendRequest);
        
        return friendRequest;
    }
    
    #region Behavior

    public Friendship? AcceptFriendRequest(Guid friendshipId)
    {
        var friendship = new Friendship
        {
            FriendshipId = friendshipId,
            FirstFriendUserProfileId = RequesterUserProfileId,
            SecondFriendUserProfileId = ReceiverUserProfileId,
            DateEstablished = DateTime.UtcNow,
            FriendshipStatus = FriendshipStatus.Active
        };

        Response = ResponseType.Accepted;
        DateResponded = DateTime.UtcNow;
        return friendship;
    }

    public void RejectFriendRequest()
    {
        Response = ResponseType.Declined;
        DateResponded = DateTime.UtcNow;
    }
    #endregion
    
}