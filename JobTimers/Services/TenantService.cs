using JobTimers.Settings;
using Microsoft.Extensions.Options;

namespace JobTimers.Services
{

    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private HttpContext? _httpContext;
        private Tenant? _currentTenant;

        public TenantService(IHttpContextAccessor contextAccessor, IOptions<TenantSettings> tenantSettings)
        {
            //acces to request
            _httpContext = contextAccessor.HttpContext;
            _tenantSettings = tenantSettings.Value;
            //testing Request
            if (_httpContext is not null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetCurrentTenant(tenantId!);
                }
                else
                {
                    throw new Exception("No tenant provided!");
                }
            }
        }
        public string? GetConnectionString()
        {
            var currentConnectionString = _currentTenant is null
                ? _tenantSettings.Defaults.ConnectionString
                : _currentTenant.ConnectionString;

            return currentConnectionString;
        }

        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
         }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Defaults.DBProvider;
        }
        //public string? GetHangfireDatabase()
        //    {
        //        return _tenantSettings.Defaults.HangfireConnection;
        //    }
        
        public void SetCurrentTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.Tid == tenantId);

            if (_currentTenant is null)
            {
                throw new Exception("Invalid tenant ID");
            }

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
            }
        }
    }
}
