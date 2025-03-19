using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Multitenancy.Infrastructure.Auth;
using Multitenancy.Infrastructure.BackgroundJobs;
using Multitenancy.Infrastructure.Caching;
using Multitenancy.Infrastructure.Common;
using Multitenancy.Infrastructure.Cors;
using Multitenancy.Infrastructure.FileStorage;
using Multitenancy.Infrastructure.Localization;
using Multitenancy.Infrastructure.Mailing;
using Multitenancy.Infrastructure.Mapping;
using Multitenancy.Infrastructure.Middleware;
using Multitenancy.Infrastructure.Multitenancy;
using Multitenancy.Infrastructure.Notifications;
using Multitenancy.Infrastructure.OpenApi;
using Multitenancy.Infrastructure.Persistence;
using Multitenancy.Infrastructure.Persistence.Initialization;
using Multitenancy.Infrastructure.SecurityHeaders;
using System.Reflection;

namespace Multitenancy.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            MapsterSettings.Configure();
            return services
                .AddApiVersioning()
                .AddAuth(config)
                .AddBackgroundJobs(config)
                .AddCaching(config)
                .AddCorsPolicy(config)
                .AddExceptionMiddleware()
                .AddHealthCheck()
                .AddLocalization(config)
                .AddMailing(config)
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddMultitenancy(config)
                .AddNotifications(config)
                .AddOpenApiDocumentation(config)
                .AddPersistence(config)
                .AddRequestLogging(config)
                .AddRouting(options => options.LowercaseUrls = true)
                .AddServices();
        }

        private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

        private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
            services.AddHealthChecks().AddCheck<TenantHealthCheck>("Tenant").Services;

        public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            // Create a new scope to retrieve scoped services
            using var scope = services.CreateScope();

            await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
                .InitializeDatabasesAsync(cancellationToken);
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
            builder
                .UseLocalization(config)
                .UseStaticFiles()
                .UseSecurityHeaders(config)
                .UseFileStorage()
                .UseExceptionMiddleware()
                .UseRouting()
                .UseCorsPolicy()
                .UseAuthentication()
                .UseCurrentUser()
                .UseMultiTenancy()
                .UseAuthorization()
                .UseRequestLogging(config)
                .UseHangfireDashboard(config)
                .UseOpenApiDocumentation(config);

        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapControllers().RequireAuthorization();
            builder.MapHealthCheck();
            builder.MapNotifications();
            return builder;
        }

        private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
            endpoints.MapHealthChecks("/api/health").RequireAuthorization();
    }
}