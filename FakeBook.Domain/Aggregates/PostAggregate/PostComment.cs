using FakeBook.Domain.Aggregates.UserProfileAggregate;
using FakeBook.Domain.Constants;
using FakeBook.Domain.ValidationExceptions;
using FakeBook.Domain.Validators.PostValidators;

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
            var validator = new PostCommentValidator();

            var comment = new PostComment()
            {
                UserProfileId = userProfileId,
                PostId = post,
                CommentText = commentText,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
            };
            var res = validator.Validate(comment);
            if (!res.IsValid)
            {
                var ex = new PostCommentNotValidException(Helper.ExceptionsMessages.PostCommentNotValidException);
                foreach (var error in res.Errors)
                {
                    ex.ValidationErrors.Add(error.ErrorMessage);
                }
                throw ex;
            }
            return comment;
        }


    }
}
