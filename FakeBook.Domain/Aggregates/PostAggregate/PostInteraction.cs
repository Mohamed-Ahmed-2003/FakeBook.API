using FakeBook.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBook.Domain.Aggregates.PostAggregate
{
    public class PostInteraction
    {
        private PostInteraction() { }
        public Guid  InteractionId { get;  private set; }
        public Guid? UserProfileId { get; private set; }
        public Guid PostId { get;  private set; }
        public ReactionType  Reaction { get;  private set; }

        public UserProfile UserProfile { get; private set; }
        public static PostInteraction CreatePostInteraction (Guid postId ,Guid userProfileId, ReactionType reaction)
        {
            return new PostInteraction() { PostId = postId, Reaction = reaction,UserProfileId= userProfileId };
        }
    }
}
