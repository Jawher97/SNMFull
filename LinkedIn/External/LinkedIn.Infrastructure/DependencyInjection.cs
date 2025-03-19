using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Infrastructure.DataContext;
using SNM.LinkedIn.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SNM.LinkedIn.Application.Interfaces;
using SNM.LinkedIn.Infrastructure.Repositories;

namespace SNM.LinkedIn.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<ILinkedInRepository<Guid>, LinkedInRepository>();
         
          
            services.AddScoped<ILinkedInChannelRepository<Guid>, LinkedInChannelRepository>();
            services.AddScoped<ILinkedInAPIRepository<Guid>, LinkedInAPIRepository>();
           
            services.AddScoped<ILinkedInProfileRepository<Guid>, LinkedInProfileRepository>();
            services.AddScoped<ILinkedInPostRepository<Guid>, LinkedInPostRepository>();
            return services;
        }
    }
}