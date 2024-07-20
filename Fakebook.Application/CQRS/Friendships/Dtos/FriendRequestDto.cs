

namespace Fakebook.Application.CQRS.Friendships.Dtos
{
    public class FriendRequestDto
    {
        public Guid FriendRequestId { get; set; }
        public Guid RequesterId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
