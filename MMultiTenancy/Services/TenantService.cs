using Microsoft.Extensions.Options;
using MultiTenancy.Settings;

namespace MultiTenancy.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private readonly HttpContext? _httpContext;
        private Tenant? _currentTenant;
        public TenantService(IHttpContextAccessor httpContextAccessor
                                ,IOptions<TenantSettings> tenantSettings)
        {
            _httpContext = httpContextAccessor.HttpContext; 
            _tenantSettings = tenantSettings.Value;

            if (_httpContext is not null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetCurrentTenant(tenantId!);
                }
                else
                    throw new ArgumentNullException("No tenant provided!");

            } 
        }

        public string? GetConnectionString()
        {
            var connectionString= _currentTenant is null ? _tenantSettings.Defaults.ConnectionString 
                                                            : _currentTenant.ConnectionString;
            return connectionString;
        }

        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
        }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Defaults.DBProvider;
        }
        void SetCurrentTenant(string tenantId)
        {
             _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t=>t.TId == tenantId);
            if (_currentTenant is null)
                throw new Exception("Invalid tenant ID");
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
        }
    }
}
