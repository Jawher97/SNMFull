using SNS.Facebook.Infrastructure.Multitenancy;

namespace SNS.Facebook.Infrastructure.Persistence.Initialization
{
    internal interface IDatabaseInitializer
    {
        Task InitializeDatabasesAsync(CancellationToken cancellationToken);
        Task InitializeApplicationDbForTenantAsync(FSHTenantInfo tenant, CancellationToken cancellationToken);
    }
}