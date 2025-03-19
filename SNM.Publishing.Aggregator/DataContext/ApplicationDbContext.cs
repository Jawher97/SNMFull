using Microsoft.EntityFrameworkCore;
using SNM.Publishing.Aggregator.Models;

namespace SNM.Publishing.Aggregator.DataContext
{
    public class ApplicationDbContext: DbContext
    {

        public DbSet<InstagramChannel> InstagramChannels { get; set; }
        public DbSet<TwitterChannel> TwitterChannel { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity relationships and constraints here...
        }
    }
}
