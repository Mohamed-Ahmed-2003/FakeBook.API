using FakeBook.Domain.Aggregates.Shared;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using FakeBook.Domain.Constants;
using FakeBook.Domain.ValidationExceptions;
using FakeBook.Domain.Validators.PostValidators;


namespace FakeBook.Domain.Aggregates.PostAggregate
{
    public class Post
    {
        private readonly List<PostComment> _comments = new List<PostComment>();
        private readonly List<PostInteraction> _interactions = new List<PostInteraction>();
        private readonly List<Media> _media = new List<Media>();

        private Post() { }

        #region Properties
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }

        public string Text { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModifiedDate { get; private set; }
        #endregion

        #region Nav Props
        public IEnumerable<PostComment> Comments => _comments; 
        public IEnumerable<PostInteraction> Interactions => _interactions;
        public IEnumerable<Media> PostMedia => _media;

        public UserProfile userProfile { get; private set; }

        #endregion

        #region FM
        public static Post CreatePost(Guid userProfileId,string text)
        {
            var validator = new PostValidator ();
            var post = new Post
            { 
                UserProfileId = userProfileId,
                Text = text,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
            };

            var res = validator.Validate(post);
            if (!res.IsValid)
            {
                var ex = new PostNotValidException (Helper.ExceptionsMessages.PostNotValidException);
                foreach (var error in res.Errors)
                {
                    ex.ValidationErrors.Add(error.ErrorMessage);
                }
                throw ex;
            }
            return post;
        }
        #endregion

        #region methods
        public void UpdatePostText(string newText)
        {
            if (string.IsNullOrEmpty(newText))
            {
                var ex = new PostNotValidException(Helper.ExceptionsMessages.PostTextNotValidException);
                ex.ValidationErrors.Add("Please provide a valid text");
                throw ex;
            }
            Text = newText;
            LastModifiedDate = DateTime.UtcNow;
        }

        public void AddComment (PostComment comment)
        {
            _comments.Add(comment);
        }
        public void RemoveComment (PostComment comment)
        {
            _comments.Remove(comment);
        }
        public void AddReaction (PostInteraction postInteraction)
        {
            _interactions.Add(postInteraction);
        }
        public void RemoveReaction (PostInteraction postInteraction)
        {
            _interactions.Remove(postInteraction);
        }

        public void AddMedia(Media media)
        {
            _media.Add(media);
            LastModifiedDate = DateTime.UtcNow;
        }

    

        public void RemoveMedia(Media media)
        {
           

            _media.Remove(media);
            LastModifiedDate = DateTime.UtcNow;
        } 
        
        public void RemoveAllMedia()
        {
           _media.Clear();
        }
        
        #endregion
    }
}
