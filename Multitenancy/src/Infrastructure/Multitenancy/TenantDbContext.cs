using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using Multitenancy.Infrastructure.Persistence.Configuration;

namespace Multitenancy.Infrastructure.Multitenancy
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