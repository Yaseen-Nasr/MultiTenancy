using Microsoft.EntityFrameworkCore;

namespace MultiTenancy.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        private string _tenantId;
        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            _tenantId = _tenantService.GetCurrentTenant()?.TId;
        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(e => e.TenatId == _tenantId);
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var dbProvider=_tenantService.GetDatabaseProvider();
                if(dbProvider!.ToLower() == "mssql") 
                {
                    optionsBuilder.UseSqlServer(tenantConnectionString);
                }
            }
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entery in ChangeTracker.Entries<IMustHaveTanent>().Where(e => e.State == EntityState.Added))
            {
                entery.Entity.TenatId = _tenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
