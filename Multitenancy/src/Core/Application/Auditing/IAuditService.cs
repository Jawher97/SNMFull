namespace Multitenancy.Application.Auditing
{
    public interface IAuditService : ITransientService
    {
        Task<List<AuditDto>> GetUserTrailsAsync(Guid userId);
    }
}