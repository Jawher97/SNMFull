using Microsoft.EntityFrameworkCore;
using SNM.Twitter.Domain.Entities;

namespace SNM.Twitter.Infrastructure.DataContext
{
    public class ApplicationDbContext : DbContext
    {
       
        public DbSet<TwitterChannel> TwitterChannel { get; set; }
        public DbSet<TwitterPost> TwitterPost { get; set; }
        //public DbSet<TwitterImages> TwitterImages { get; set; }
        //public DbSet<TwitterProfileData> TwitterProfileData { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}