
using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.Posts.Requests;
public class PostCommentCreate
{
        public required string Text { get;  set; }
    
}