

namespace Fakebook.Application.CQRS.Friendships.Dtos
{
    public class FriendDto
    {
        public Guid FriendId { get; set; }
        public DateTime FriendshipStartedAt { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }

    }


}
