using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.Posts.Requests
{
    public class PostCreate
    {
        [Required]
        public Guid UserProfileId { get; set; }

        [Length(1,2000)]
        public required string Text { get; set; }
    }
}
