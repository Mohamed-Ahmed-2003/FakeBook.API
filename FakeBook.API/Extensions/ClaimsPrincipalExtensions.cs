using System.Security.Claims;

namespace FakeBook.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
    
        public static Guid GetClaimAsGuid(this ClaimsPrincipal user, string claimType)
        {
            var userIdentity = user.Identity as ClaimsIdentity;
            var claimValue = userIdentity?.FindFirst(claimType)?.Value;

            if (Guid.TryParse(claimValue, out var result))
            {
                return result;
            }

            throw new InvalidOperationException($"Claim '{claimType}' is missing or invalid.");
        }

        public static Guid GetUserProfileId(this ClaimsPrincipal user)
        {
            return user.GetClaimAsGuid("ProfileId");
        }

        public static Guid GetIdentityUserId(this ClaimsPrincipal user)
        {
            return user.GetClaimAsGuid("UserId");
        }
    }
}
