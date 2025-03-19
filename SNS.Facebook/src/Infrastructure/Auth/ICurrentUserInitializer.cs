using System.Security.Claims;

namespace SNS.Facebook.Infrastructure.Auth
{
    public interface ICurrentUserInitializer
    {
        void SetCurrentUser(ClaimsPrincipal user);

        void SetCurrentUserId(string userId);
    }
}