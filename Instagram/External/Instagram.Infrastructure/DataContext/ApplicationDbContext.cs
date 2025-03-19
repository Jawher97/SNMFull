using SNM.Instagram.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SNM.Instagram.Domain.Common;

namespace SNM.Instagram.Infrastructure.DataContext
{
    public class ApplicationDbContext : DbContext
    {
    
        public DbSet<InstagramChannel> InstagramChannels { get; set; }
        public DbSet<InstagramPost> InstagramPosts { get; set; }

        //public DbSet<InstagramImage> InstagramImages { get; set; }

        //public DbSet<InstagramChannelPost> InstagramChannelPost { get; set; }
        //public DbSet<Insight> Insight { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<InstagramPost>()
            //  .HasMany(q => q.InstagramImages)
            //  .WithOne(r => r.InstagramPost)
            //  .HasForeignKey(r => r.InstagramPostlId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}