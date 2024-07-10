using FakeBook.API.Filters;
using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.Identity.Requests
{
    public class UserRegister
    {
        [EmailAddress]
        public required string Username { get; set; }
 
        [Length(3,50)]
        public required string FirstName { get; set; }

        [Length(3, 50)]
        public required string LastName { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; }


        [DateOfBirthRange(18,100)]
        public DateTime DateOfBirth { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public string? City { get; set; }
    }
}
