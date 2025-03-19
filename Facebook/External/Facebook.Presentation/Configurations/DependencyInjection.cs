using Microsoft.Extensions.DependencyInjection;

namespace SNS.Facebook.Presentation.Configurations
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