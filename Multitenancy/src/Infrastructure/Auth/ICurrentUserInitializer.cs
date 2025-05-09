﻿using System.Security.Claims;

namespace Multitenancy.Infrastructure.Auth
{
    public interface ICurrentUserInitializer
    {
        void SetCurrentUser(ClaimsPrincipal user);

        void SetCurrentUserId(string userId);
    }
}