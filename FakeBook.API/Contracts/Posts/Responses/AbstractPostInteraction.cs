using FakeBook.Domain.Aggregates.Shared;

namespace FakeBook.API.Contracts.Posts.Responses
{
    public class InteractionAuthor
    {
        public Guid UserProfileId { get; set; }
        public required string FullName { get; set; }
        public string? City { get; set; }
    }
    public class AbstractPostInteraction
    {
        public Guid InteractionId { get; set; }
        public required string Type { get; set; }
        public required InteractionAuthor Author { get; set; }
        public Media? ProfilePicture { get; private set; }
    }
}
