using FakeBook.Domain.Aggregates.Shared;
using FakeBook.Domain.Aggregates.UserProfileAggregate;

namespace FakeBook.API.Contracts.UserProfile.Responses
{
    public class UserProfileResponse
    {
        public Guid UserProfileId { get;  set; }
        public GeneralInfoResponse GeneralInfo { get;  set; }
        public DateTime DateCreated { get;  set; }
        public DateTime LastModified { get;  set; }
        public Media? ProfilePicture { get;  set; }
        public Media? ProfileCoverImage { get;  set; }

    }
}
