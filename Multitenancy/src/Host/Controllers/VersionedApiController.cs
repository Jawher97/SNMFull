﻿namespace Multitenancy.Host.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionedApiController : BaseApiController
    {
    }
}