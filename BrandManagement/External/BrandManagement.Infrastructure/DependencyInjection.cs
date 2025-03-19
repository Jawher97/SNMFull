using SNM.BrandManagement.Application.Interfaces;
using SNM.BrandManagement.Infrastructure.DataContext;
using SNM.BrandManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SNM.BrandManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<IBrandRepository<Guid>, BrandRepository>();
            services.AddScoped<IChannelRepository<Guid>, ChannelRepository>();
            services.AddScoped<IChannelTypeRepository<Guid>, ChannelTypeRepository>();
            services.AddScoped<IPostRepository<Guid>, PostRepository>();
            services.AddScoped<IChannelProfileRepository<Guid>, ChannelProfileRepository>();
            services.AddScoped<IMediaRepository<Guid>, MediaRepository>();

            return services;
        }
    }
}