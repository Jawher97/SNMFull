using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching
{
    public class LocalCacheService : CacheService<Multitenancy.Infrastructure.Caching.LocalCacheService>
    {
        protected override Multitenancy.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
            new(
                new MemoryCache(new MemoryCacheOptions()),
                NullLogger<Multitenancy.Infrastructure.Caching.LocalCacheService>.Instance);
    }
}