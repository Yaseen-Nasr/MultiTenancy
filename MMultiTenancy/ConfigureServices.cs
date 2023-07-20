namespace MultiTenancy
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddTenancy(this IServiceCollection services
                                                                            ,ConfigurationManager configuration)
        {
            services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));

            services.AddScoped<ITenantService, TenantService>();
            TenantSettings options = new();

            configuration.GetSection(nameof(TenantSettings)).Bind(options);
            var defaultDatabaseProvider = options.Defaults.DBProvider;
            if (defaultDatabaseProvider.ToLower() == "mssql")
            {
                services.AddDbContext<AppDbContext>(m => m.UseSqlServer());
            }

            foreach (var tenant in options.Tenants)
            {
                var connectionString = tenant.ConnectionString ?? options.Defaults.ConnectionString;
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.SetConnectionString(connectionString);
                if (dbContext.Database.GetPendingMigrations().Any())
                    dbContext.Database.Migrate();

            }

            return services;
        }
    }
}
