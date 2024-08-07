﻿using FakeBook.Domain.Aggregates.UserProfileAggregate;


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
