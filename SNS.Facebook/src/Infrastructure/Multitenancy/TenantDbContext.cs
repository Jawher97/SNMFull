using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using SNS.Facebook.Infrastructure.Persistence.Configuration;

namespace SNS.Facebook.Infrastructure.Multitenancy
{
    public class TenantDbContext : EFCoreStoreDbContext<FSHTenantInfo>
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FSHTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
        }
    }
}