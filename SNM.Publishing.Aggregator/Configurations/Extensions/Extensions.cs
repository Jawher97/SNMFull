using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Services;
using System.Text.Json;
    

namespace SNM.Publishing.Aggregator.Configurations.Extensions
{
    public static class Extensions
    {
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                //.AddUrlGroup(_ => new Uri(configuration.GetRequiredValue("CatalogUrlHC")), name: "catalogapi-check", tags: new string[] { "catalogapi" })
                .AddUrlGroup(new Uri($"{configuration["ApiSettings:BrandUrl"]}/swagger/index.html"), "Brand.API", HealthStatus.Degraded)
                .AddUrlGroup(new Uri($"{configuration["ApiSettings:FacebookUrl"]}/swagger/index.html"), "Facebook.API", HealthStatus.Degraded)
                .AddUrlGroup(new Uri($"{configuration["ApiSettings:InstagramUrl"]}/swagger/index.html"), "Instagram.API", HealthStatus.Degraded)
                .AddUrlGroup(new Uri($"{configuration["ApiSettings:LinkedInUrl"]}/swagger/index.html"), "LinkedIn.API", HealthStatus.Degraded)
                .AddUrlGroup(new Uri($"{configuration["ApiSettings:TwitterUrl"]}/swagger/index.html"), "Twitter.API", HealthStatus.Degraded);
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpClient<IBrandService, BrandService>(c =>
           c.BaseAddress = new Uri(configuration["ApiSettings:BrandUrl"]));

            services.AddHttpClient<IPostFacebookService, PostFacebookService>(c => 
            c.BaseAddress = new Uri(configuration["ApiSettings:FacebookUrl"]));
            //   .AddHttpMessageHandler<LoggingDelegatingHandler>()
            //.AddPolicyHandler(GetRetryPolicy())
             //.AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IPostLinkedInService, PostLinkedInService>(c => 
            c.BaseAddress = new Uri(configuration["ApiSettings:LinkedInUrl"]))
            // .AddHttpMessageHandler<LoggingDelegatingHandler>()
              .AddPolicyHandler(GetRetryPolicy())
              .AddPolicyHandler(GetCircuitBreakerPolicy());
            services.AddHttpClient<IPostInstagramServices, PostInstagramServices>(c =>
           c.BaseAddress = new Uri(configuration["ApiSettings:InstagramUrl"]))
             // .AddHttpMessageHandler<LoggingDelegatingHandler>()
             .AddPolicyHandler(GetRetryPolicy())
             .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IPostTwitterServices, PostTwitterService>(c =>
         c.BaseAddress = new Uri(configuration["ApiSettings:TwitterUrl"]))
           // .AddHttpMessageHandler<LoggingDelegatingHandler>()
           .AddPolicyHandler(GetRetryPolicy())
           .AddPolicyHandler(GetCircuitBreakerPolicy());
            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // In this case will wait for
            //  2 ^ 1 = 2 seconds then
            //  2 ^ 2 = 4 seconds then
            //  2 ^ 3 = 8 seconds then
            //  2 ^ 4 = 16 seconds then
            //  2 ^ 5 = 32 seconds

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryCount, context) =>
                    {
                         Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                    });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30)
                );
        }
    }
}
