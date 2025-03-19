using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SNS.Facebook.Infrastructure.Common.Services;

namespace Infrastructure.Test.Caching
{
    public class DistributedCacheService : CacheService<SNS.Facebook.Infrastructure.Caching.DistributedCacheService>
    {
        protected override SNS.Facebook.Infrastructure.Caching.DistributedCacheService CreateCacheService() =>
            new(
                new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())),
                new NewtonSoftService(),
                NullLogger<SNS.Facebook.Infrastructure.Caching.DistributedCacheService>.Instance);
    }
}