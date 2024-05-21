using FakeBook.Domain.Aggregates.UserProfileAggregate;

namespace FakeBook.Domain.Aggregates.PostAggregate
{
    public class PostComment
    {
        private PostComment() { }
        public Guid CommentId { get; private set; }
        public Guid PostId { get;  private set; }
        public Guid UserProfileId { get; private set; }
        public string CommentText { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModifiedDate { get; private set; }

        public UserProfile UserProfile { get; private set; }
        public Post Post { get; private set; }

        public static PostComment CreatePostComment (Guid userProfileId , Guid post , string commentText)
        {
            return new PostComment
            {
                UserProfileId = userProfileId,
                PostId = post,
                CommentText = commentText,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
            };
        }


    }
}
