using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Fakebook.Application.Account.Commands
{
    public class UpdateUserCmd : IRequest<Response<UserProfile>>
    {
        public required string  UserProfileId { get;  set; }
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
