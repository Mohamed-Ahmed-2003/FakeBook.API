using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.Posts.Requests
{
    public class PostCreate
    {
       

        [Length(1,2000)]
        public required string Text { get; set; }
        public List<IFormFile>? MediaFiles { get; set; }
    }
}
