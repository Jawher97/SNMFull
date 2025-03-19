using JobTimers.Contracts;

using JobTimers.models;
using JobTimers.Services;
using Microsoft.EntityFrameworkCore;

namespace JobTimers
{
    public class ApplicationDbContext : DbContext
    {
        public string TenantId { get; set; }


      //  private readonly IHangfireTenantProvider _hfTenantProvider;
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            //if (tenantService.GetCurrentTenant() == null && !string.IsNullOrWhiteSpace(hfTenantProvider.HfGetTenantId()))
            //{
            //    tenantService.SetCurrentTenant(hfTenantProvider.HfGetTenantId());
            //}
            //_tenantService = tenantService;
            //TenantId = _tenantService.GetCurrentTenant()?.Tid;
           // _hfTenantProvider = hfTenantProvider;
        }
        //public DbSet<LinkedInPost> LinkedInPost { get; set; }
        //public DbSet<InstagramPostDto> InstagramPost { get; set; }
        //public DbSet<TwitterPost> TwitterPost { get; set; }
        //public DbSet<FacebookPostDto> facebookPost { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // when receive data filter with current tenantId
            //modelBuilder.Entity<LinkedInPost>().HasQueryFilter(e => e.TenantId == TenantId);
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<InstagramPost>().HasQueryFilter(e => e.TenantId == TenantId);
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<TwitterPost>().HasQueryFilter(e => e.TenantId == TenantId);
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<FacebookPost>().HasQueryFilter(e => e.TenantId == TenantId);
            //base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var tenantConnectionString = _tenantService.GetConnectionString();
            ////var HangfireConnection = _tenantService.GetHangfireDatabase();

            //if (!string.IsNullOrWhiteSpace(tenantConnectionString))
            //{
            //    var dbProvider = _tenantService.GetDatabaseProvider();

            //    if (dbProvider?.ToLower() == "mysql")
            //    {
             
            //        optionsBuilder.UseMySql(tenantConnectionString, ServerVersion.AutoDetect(tenantConnectionString));

            //        //optionsBuilder.UseMySql(HangfireConnection, ServerVersion.AutoDetect(HangfireConnection));


            //    }
            //}
        }

        // overide you are providing a new implementation for a method that is already defined in a base class (or interface)
        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    // all entity implement interface IMustHaveTenant added curent tenant id with every new item
        //    foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
        //    {
        //        entry.Entity.TenantId = TenantId;
        //    }

        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }
}
