using SNM.LinkedIn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SNM.LinkedIn.Infrastructure.DataContext
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<LinkedInChannel> LinkedInChannel { get; set; }
        public DbSet<LinkedInPost> LinkedInPost { get; set; }
     //   public DbSet<LinkedInComment> LinkedInComments { get; set; }
      //  public DbSet<LinkedInChannelPost> LinkedInChannelPost { get; set; }
        //public DbSet<LinkedInArticle> LinkedInArticle { get; set; }
        public DbSet<LinkedInProfileData> LinkedInProfileData { get; set; }
        //public DbSet<LinkedInInsight> linkedInInsights { get; set; }
       // public DbSet<MediaLinkedin> MediaLinkedin { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<LinkedInProfileData>()
            //.HasMany(q => q.LinkedinPosts)
            //.WithOne(r => r.Profile)
            //.HasForeignKey(r => r.LinkedInProfileDataId).OnDelete(DeleteBehavior.Cascade);
            //        modelBuilder.Entity<LinkedInProfileData>()
            //.HasOne(p => p.linkedInPost)
            //.WithOne(p => p.Profile) // Specify the navigation property on the other side of the relationship
            //.HasForeignKey<LinkedInPost>(p => p.ProfileId);
            //modelBuilder.Entity<LinkedInPost>()
            //        .HasMany(q => q.MediaLinkedin)
            //        .WithOne(r => r.LinkedInPost)
            //        .HasForeignKey(r => r.LinkedinPostId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LinkedInPost>()
           .HasOne(e => e.LinkedInChannel)
           .WithMany()
           .HasForeignKey(e => e.LinkedInChannelId);
         



        }
    }
}