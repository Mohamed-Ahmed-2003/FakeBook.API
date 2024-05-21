using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBook.Domain.Aggregates.UserProfileAggregate
{
    public class UserProfile
    {
        private UserProfile() { }

        public Guid UserProfileId { get; set; }
        public string IdentityId { get; private set; }
        public GeneralInfo GeneralInfo { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime LastModified { get; private set; }

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
    }
}
