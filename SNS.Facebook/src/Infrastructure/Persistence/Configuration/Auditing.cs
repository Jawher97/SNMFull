using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SNS.Facebook.Infrastructure.Auditing;

namespace SNS.Facebook.Infrastructure.Persistence.Configuration
{
    public class AuditTrailConfig : IEntityTypeConfiguration<Trail>
    {
        public void Configure(EntityTypeBuilder<Trail> builder) =>
            builder
                .ToTable("AuditTrails", SchemaNames.Auditing)
                .IsMultiTenant();
    }
}