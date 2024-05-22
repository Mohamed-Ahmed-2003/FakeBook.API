using FakeBook.Domain.Aggregates.UserProfileAggregate;
using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Contracts.UserProfile.Requests
{
    public class UserProfileCreateUpdate
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
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public string? City { get; set; }

    }
}
