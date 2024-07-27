

using FakeBook.Domain.Aggregates.Shared;

namespace FakeBook.Domain.Aggregates.UserProfileAggregate
{
    public class UserProfile
    {
        private UserProfile() { }

        public Guid UserProfileId { get; private set; }
        public string IdentityId { get; private set; }
        public GeneralInfo GeneralInfo { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime LastModified { get; private set; }
        public DateTime LastActive { get; private set; }
        public string ConnectionId { get; private set; }
        public Media ? ProfilePicture { get; private set; }
        public Media ? ProfileCoverImage { get; private set; }

        public static UserProfile CreateUserProfile(string identityId, GeneralInfo generalInfo)
        {

            return new UserProfile
            {
                IdentityId = identityId,
                GeneralInfo = generalInfo,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
        }
        //public methods
        public void UpdateBasicInfo(GeneralInfo newInfo)
        {
            GeneralInfo = newInfo;
            LastModified = DateTime.UtcNow;
        }
        public void Online (string connId)
        {
            ConnectionId = connId;
        }
        public void Offline ()
        {
            ConnectionId = string.Empty;
            LastActive = DateTime.UtcNow;
        }
        public string GetFullName ()
        {
            return GeneralInfo.FirstName + " "+GeneralInfo.LastName;
        }
        public void SetProfilePicture(Media media)
        {
            ProfilePicture = media;
            LastModified = DateTime.UtcNow;
        }

        public void SetProfileCoverImage(Media media)
        {
            ProfileCoverImage = media;
            LastModified = DateTime.UtcNow;
        }

        public void UpdateProfilePicture(string url, MediaType mediaType)
        {
            if (ProfilePicture == null)
            {
                throw new InvalidOperationException("Profile picture not set.");
            }

            ProfilePicture.UpdateDetails(url, mediaType);
            LastModified = DateTime.UtcNow;
        }

        public void UpdateProfileCoverImage(string url, MediaType mediaType)
        {
            if (ProfileCoverImage == null)
            {
                throw new InvalidOperationException("Profile cover image not set.");
            }

            ProfileCoverImage.UpdateDetails(url, mediaType);
            LastModified = DateTime.UtcNow;
        }

        public void RemoveProfilePicture()
        {
            ProfilePicture = null;
            LastModified = DateTime.UtcNow;
        }

        public void RemoveProfileCoverImage()
        {
            ProfileCoverImage = null;
            LastModified = DateTime.UtcNow;
        }
    }
}
