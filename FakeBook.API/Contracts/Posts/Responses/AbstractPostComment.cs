
namespace FakeBook.API.Contracts.Posts.Responses;
public class AbstractPostComment
{
    public Guid CommentId { get; set; }
    public string Text { get;  set; }
    public string UserProfileId { get; set; }
}