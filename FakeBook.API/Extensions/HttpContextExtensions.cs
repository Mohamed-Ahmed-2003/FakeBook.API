using System.Security.Claims;

namespace FakeBook.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetClaimAsGuid(this HttpContext context, string claimType)
        {
            var userIdentity = context.User.Identity as ClaimsIdentity;
            var claimValue = userIdentity?.FindFirst(claimType)?.Value;

            if (Guid.TryParse(claimValue, out var result))
            {
                return result;
            }

            throw new InvalidOperationException($"Claim '{claimType}' is missing or invalid.");
        }

        public static Guid GetUserProfileId(this HttpContext context)
        {
            return context.GetClaimAsGuid("ProfileId");
        }

        public static Guid GetIdentityUserId(this HttpContext context)
        {
            return context.GetClaimAsGuid("UserId");
        }
    }
}
