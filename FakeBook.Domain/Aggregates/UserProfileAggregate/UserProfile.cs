

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
    }
}
