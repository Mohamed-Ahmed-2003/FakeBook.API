using Fakebook.Application.CQRS.Account.Dtos;
using Fakebook.Application.Generics;
using MediatR;


namespace Fakebook.Application.CQRS.Account.Commands
{
    public class UpdateUserCmd : IRequest<Response<IdentityUserProfileDto>>
    {
        public required Guid UserProfileId { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }


        public required string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
    }
}
