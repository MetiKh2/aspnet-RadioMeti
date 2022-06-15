using System.Security.Claims;

namespace RadioMeti.Site.Utilities.Identity
{
    public static class IdentityExtensions
    {
        public static string UserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var data = claimsPrincipal.Claims.SingleOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
                if (data != null)
                    return data.Value;
            }
            return default(string);
        }
    }
}
