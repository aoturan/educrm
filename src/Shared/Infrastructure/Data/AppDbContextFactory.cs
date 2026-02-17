using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EduCrm.Infrastructure.Data;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Prefer env var for tooling:
        // ConnectionStrings__Default="Host=...;Database=...;Username=...;Password=..."
        var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                 ?? "Host=localhost;Port=5432;Database=educrm;Username=educrm;Password=educrm_dev_password;Include Error Detail=true";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(cs, npgsql => npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
            .Options;

        return new AppDbContext(options);
    }
}