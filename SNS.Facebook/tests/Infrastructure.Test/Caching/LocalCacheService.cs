using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching
{
    public class LocalCacheService : CacheService<SNS.Facebook.Infrastructure.Caching.LocalCacheService>
    {
        protected override SNS.Facebook.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
            new(
                new MemoryCache(new MemoryCacheOptions()),
                NullLogger<SNS.Facebook.Infrastructure.Caching.LocalCacheService>.Instance);
    }
}