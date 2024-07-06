using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.Posts.Requests;
public class PostCommentUpdate
{
    [Required]
    public string Text { get;  set; }
}