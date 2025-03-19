using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Multitenancy.Infrastructure.Common.Services;

namespace Infrastructure.Test.Caching
{
    public class DistributedCacheService : CacheService<Multitenancy.Infrastructure.Caching.DistributedCacheService>
    {
        protected override Multitenancy.Infrastructure.Caching.DistributedCacheService CreateCacheService() =>
            new(
                new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())),
                new NewtonSoftService(),
                NullLogger<Multitenancy.Infrastructure.Caching.DistributedCacheService>.Instance);
    }
}