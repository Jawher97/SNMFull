using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SNS.Facebook.Application.Common.Caching;

namespace SNS.Facebook.Infrastructure.Caching
{
    internal static class Startup
    {
        internal static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration config)
        {
            var settings = config.GetSection(nameof(CacheSettings)).Get<CacheSettings>();

            if (settings.UseDistributedCache)
            {
                if (settings.PreferRedis)
                {
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = settings.RedisURL;
                        options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                        {
                            AbortOnConnectFail = true,
                            EndPoints = { settings.RedisURL }
                        };
                    });
                }
                else
                {
                    services.AddDistributedMemoryCache();
                }

                services.AddTransient<ICacheService, DistributedCacheService>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddTransient<ICacheService, LocalCacheService>();
            }

            return services;
        }
    }
}