using JobTimers.Settings;

namespace JobTimers.Services
{
    public interface ITenantService
    {
        string? GetDatabaseProvider();
        string? GetConnectionString();
        Tenant? GetCurrentTenant();
        void SetCurrentTenant(string tenantId);
      
        // string? GetHangfireDatabase();
    }
}
