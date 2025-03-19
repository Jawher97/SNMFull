using Multitenancy.Infrastructure.Multitenancy;

namespace Multitenancy.Infrastructure.Persistence.Initialization
{
    internal interface IDatabaseInitializer
    {
        Task InitializeDatabasesAsync(CancellationToken cancellationToken);
        Task InitializeApplicationDbForTenantAsync(FSHTenantInfo tenant, CancellationToken cancellationToken);
    }
}