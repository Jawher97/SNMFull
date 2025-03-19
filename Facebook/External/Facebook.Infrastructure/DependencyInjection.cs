using SNS.Facebook.Application.Interfaces;
using SNS.Facebook.Infrastructure.DataContext;
using SNS.Facebook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SNS.Facebook.Application.DTO;

namespace SNS.Facebook.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<IFacebookAPIRepository, FacebookAPIRepository>();
            services.AddScoped<IFacebookPostRepository<Guid>, FacebookPostRepository>();
            services.AddScoped<IFacebookChannelRepository<Guid>, FacebookChannelRepository>();
            services.AddScoped<IFacebookRepository<Guid>, FacebookRepository>();

            return services;
        }
    }
}