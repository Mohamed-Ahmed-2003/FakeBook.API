using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.CQRS.Posts
{
    public class PostsErrorMessages
    {
        public const string PostNotFound = "Post with ID {0} not found.";
        public const string PostDeleteNotPossible = "Only the post owner can delete it.";
        public const string PostUpdateNotPossible = "Only the post owner can update it.";
        public const string PostInteractionNotFound = "Interaction not found.";
        public const string InteractionRemovalNotAuthorized = "Only the interaction author can remove it.";
        public const string PostCommentNotFound = "Comment not found.";
        public const string CommentUpdateNotAuthorized = "Only the comment author can update it.";
        public const string CommentRemovalNotAuthorized = "Only the comment author can remove it.";
    }

}
