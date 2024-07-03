namespace FakeBook.API.Contracts.Posts.Responses
{
    public class AbstractPostResponse
    {
        public Guid PostId { get;  set; }
        public Guid UserProfileId { get;  set; }

        public required string Text { get;  set; }
        public DateTime CreatedDate { get;  set; }
        public DateTime LastModifiedDate { get;  set; }
    }
}
