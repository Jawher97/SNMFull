using Microsoft.AspNetCore.Authorization;
using Multitenancy.Shared.Authorization;

namespace Multitenancy.Infrastructure.Auth.Permissions
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) =>
            Policy = FSHPermission.NameFor(action, resource);
    }
}