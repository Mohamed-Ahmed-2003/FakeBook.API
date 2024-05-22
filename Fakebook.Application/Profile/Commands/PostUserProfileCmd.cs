using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Fakebook.Application.Profile.Commands
{
    public class PostUserProfileCmd : IRequest<UserProfile>
    {
        public Guid UserProfileId { get;  set; }
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string EmailAddress { get;  set; }
        public DateTime DateOfBirth { get;  set; }
        public string Phone { get;  set; }
        public string City { get;  set; }
    }
}
