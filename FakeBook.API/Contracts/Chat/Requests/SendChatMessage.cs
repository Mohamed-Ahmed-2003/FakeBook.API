using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.Chat.Requests
{
    public class SendChatMessage
    {
        [Required]
        [Length(1, 255)]
        public required string Content { get; set; }
    }
}
