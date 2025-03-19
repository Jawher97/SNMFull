using SNM.Instagram.Application.Interfaces;
using SNM.Instagram.Infrastructure.DataContext;
using SNM.Instagram.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SNM.Instagram.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            
            services.AddScoped<IInstagramRepository<Guid>, InstagramRepository>();
            services.AddScoped<IBrandRepository<Guid>, BrandRepository>();
            services.AddScoped<IChannelRepository<Guid>, ChannelRepository>();
            services.AddScoped<IInstagramChannelRepository<Guid>, InstagramChannelRepository>();
            services.AddScoped<IPostRepository<Guid>, PostRepository>();
            services.AddScoped<IChannelPostRepository<Guid>, ChannelPostRepository>();
            services.AddScoped<IInstagramAPIRepository<Guid>, InstagramAPIRepository>();
            services.AddScoped<IInstagramPostRepository<Guid>, InstagramPostRepository>();
            services.AddScoped<IInstagramPostApiRepository, InstagramPostAPIRepository>();
            services.AddScoped<IInstagramImagesRepository<Guid>, InstagramImagesRepository>();


            services.AddScoped<IInsightRepository, InsightRepository>();
            services.AddScoped<IInstagramProfileDataRepository<Guid>, InstagramProfileDataRepository>();


            // Récupérer le token d'accès à partir de la configuration

            var accessToken = configuration["Instagram:AccessToken"];

            services.AddScoped<IInstagramPublishService>(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                return new InstagramPublishService(configuration);
            });
           

            services.AddSingleton(configuration.GetConnectionString("DefaultConnection"));


            return services;
        }
    }
}