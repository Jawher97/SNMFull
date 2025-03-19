using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly.Extensions.Http;
using Polly;
using SNM.Publishing.Aggregator.Interfaces;
using SNM.Publishing.Aggregator.Services;
using System.Text.Json;
using Serilog;

namespace SNM.Publishing.Aggregator.Configurations
{
    public static class DependencyInjection
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

    }
}
