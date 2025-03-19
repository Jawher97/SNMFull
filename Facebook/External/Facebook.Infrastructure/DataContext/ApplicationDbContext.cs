using SNS.Facebook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SNS.Facebook.Infrastructure.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FacebookPost> FacebookPost { get; set; }
        public DbSet<FacebookChannel> FacebookChannel { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}