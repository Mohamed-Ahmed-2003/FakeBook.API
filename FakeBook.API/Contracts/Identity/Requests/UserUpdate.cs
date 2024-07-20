using FakeBook.API.Filters;
using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.Identity.Requests
{
    public class UserUpdate
    {
        [Length(4, 25)]
        [Required]
        public required string FirstName { get; set; }
        [Length(4, 25)]
        [Required]
        public required string LastName { get; set; }

        [EmailAddress]
        [Required]
        public required string EmailAddress { get; set; }

        [DateOfBirthRange(18, 100)]
        public DateTime DateOfBirth { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public string? City { get; set; }
    }
}
