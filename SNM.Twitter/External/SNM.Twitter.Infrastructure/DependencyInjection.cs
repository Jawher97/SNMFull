using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SNM.Twitter.Application.Interfaces;
using SNM.Twitter.Infrastructure.DataContext;
using SNM.Twitter.Infrastructure.Repositories;

namespace SNM.Twitter.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            services.AddScoped<ItwitterRepository<Guid>, TwitterRepository>();
            services.AddScoped<ITwitterAPIRepository<Guid>, TwitterAPIRepository>();
            services.AddScoped<ITwitterOAuthRepository, TwitterOauthRepository>();
            services.AddScoped<ITwitterOAuth2Repository, TwitterOAuth2Repository>();
            services.AddScoped<ITwitterChannelRepository<Guid>, TwitterChannelRepository>();
            services.AddScoped<ITwitterProfileDataRepository<Guid>, TwitterProfileDataRepository>();
            services.AddScoped<ITwitterPostApiRepository, TwitterPostApiRepository>();
            services.AddScoped<ITwitterPostRepository<Guid> , TwitterPostRepository>();
          
            return services;
        }
    }
}