using EduCrm.Infrastructure.Data;
using EduCrm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Infrastructure.Extensions;

public static class DataServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresDb(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("ConnectionStrings:Default is missing.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(cs, npgsql =>
            {
                // ✅ Migration strategy: store migrations in EduCrm.Infrastructure assembly
                npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
            });
        });
        
        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<AppDbContext>());

        return services;
    }
}