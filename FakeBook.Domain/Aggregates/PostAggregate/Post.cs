using FakeBook.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBook.Domain.Aggregates.PostAggregate
{
    public class Post
    {
        private readonly List<PostComment> _comments = new List<PostComment>();
        private readonly List<PostInteraction> _interactions = new List<PostInteraction>();

        private Post() { }

        #region Properties
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }

        public string Text { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModifiedDate { get; private set; }
        #endregion

        #region Nav Props
        public IEnumerable<PostComment> Comments { get { return _comments; } }
        public IEnumerable<PostInteraction> Interactions { get { return _interactions; } }
        public UserProfile userProfile { get; private set; }
        #endregion

        #region FM
        public static Post CreatePost(Guid userProfileId,string text)
        {
            return new Post
            { 
                UserProfileId = userProfileId,
                Text = text,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
            };

        }
        #endregion


        public void UpdatePostText(string newText)
        {
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
    }
}
