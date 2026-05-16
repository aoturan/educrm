using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Infrastructure.RateLimiting;

public static class RateLimitingServiceCollectionExtensions
{
    public static IServiceCollection AddRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RateLimitOptions>(configuration.GetSection("RateLimits"));

        services.AddScoped<IRateLimitCounterRepository, RateLimitCounterRepository>();
        services.AddScoped<IRateLimiter, RateLimiter>();

        return services;
    }
}
