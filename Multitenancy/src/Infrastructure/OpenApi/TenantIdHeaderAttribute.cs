﻿using Multitenancy.Shared.Multitenancy;

namespace Multitenancy.Infrastructure.OpenApi
{
    public class TenantIdHeaderAttribute : SwaggerHeaderAttribute
    {
        public TenantIdHeaderAttribute()
            : base(
                MultitenancyConstants.TenantIdName,
                "Input your tenant Id to access this API",
                string.Empty,
                true)
        {
        }
    }
}