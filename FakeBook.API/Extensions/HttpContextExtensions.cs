using System.Security.Claims;

namespace FakeBook.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetUserProfileId(this HttpContext context)
        {
            var userIdentity = context.User.Identity as ClaimsIdentity;
            var profileId = userIdentity?.FindFirst("ProfileId")?.Value;

            return Guid.Parse(profileId);
        }
    }
}
