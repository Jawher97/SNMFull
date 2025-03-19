using Microsoft.AspNetCore.Authorization;
using SNS.Facebook.Shared.Authorization;

namespace SNS.Facebook.Infrastructure.Auth.Permissions
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) =>
            Policy = FSHPermission.NameFor(action, resource);
    }
}