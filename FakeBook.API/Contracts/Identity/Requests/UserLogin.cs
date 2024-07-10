using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.Identity.Requests
{
    public class UserLogin
    {
        [EmailAddress]
        public required string Username { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
