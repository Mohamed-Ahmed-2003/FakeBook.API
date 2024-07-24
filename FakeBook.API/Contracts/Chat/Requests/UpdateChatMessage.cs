using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.Chat.Requests
{
    public class UpdateChatMessage
    {
        [Required]
        public string NewContent { get; set; }
    }
}
