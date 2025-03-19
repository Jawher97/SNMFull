using SNM.BrandManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SNM.BrandManagement.Infrastructure.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Post> Post { get; set; } 
        public DbSet<Media> Media { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Reactions> Reactions { get; set; }
        public DbSet<ChannelProfile> ChannelProfile { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>()
                .HasOne(c => c.ChannelProfile)
                .WithMany(c => c.Channel)
                .HasForeignKey(c => c.ChannelProfileId);
            modelBuilder.Entity<Comment>()
      .HasMany(q => q.Replies)
      .WithOne(a => a.Reply)
      .HasForeignKey(r => r.RepliesId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}