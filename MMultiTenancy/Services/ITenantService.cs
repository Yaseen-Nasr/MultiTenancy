namespace MultiTenancy.Services
{
    public interface ITenantService
    {
        string? GetConnectionString();
        string? GetDatabaseProvider();
        Tenant ? GetCurrentTenant(); 
    }
}
