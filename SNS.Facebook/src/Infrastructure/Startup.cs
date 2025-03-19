using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SNS.Facebook.Infrastructure.Auth;
using SNS.Facebook.Infrastructure.BackgroundJobs;
using SNS.Facebook.Infrastructure.Caching;
using SNS.Facebook.Infrastructure.Common;
using SNS.Facebook.Infrastructure.Cors;
using SNS.Facebook.Infrastructure.FileStorage;
using SNS.Facebook.Infrastructure.Localization;
using SNS.Facebook.Infrastructure.Mailing;
using SNS.Facebook.Infrastructure.Mapping;
using SNS.Facebook.Infrastructure.Middleware;
using SNS.Facebook.Infrastructure.Multitenancy;
using SNS.Facebook.Infrastructure.Notifications;
using SNS.Facebook.Infrastructure.OpenApi;
using SNS.Facebook.Infrastructure.Persistence;
using SNS.Facebook.Infrastructure.Persistence.Initialization;
using SNS.Facebook.Infrastructure.SecurityHeaders;
using System.Reflection;

namespace SNS.Facebook.Infrastructure
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