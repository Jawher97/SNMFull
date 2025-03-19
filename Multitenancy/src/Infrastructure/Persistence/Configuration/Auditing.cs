using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multitenancy.Infrastructure.Auditing;

namespace Multitenancy.Infrastructure.Persistence.Configuration
{
    public class AuditTrailConfig : IEntityTypeConfiguration<Trail>
    {
        public void Configure(EntityTypeBuilder<Trail> builder) =>
            builder
                .ToTable("AuditTrails", SchemaNames.Auditing)
                .IsMultiTenant();
    }
}