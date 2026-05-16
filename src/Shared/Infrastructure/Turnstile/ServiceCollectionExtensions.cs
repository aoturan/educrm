using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Infrastructure.Turnstile;

public static class TurnstileServiceCollectionExtensions
{
    public static IServiceCollection AddTurnstile(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TurnstileOptions>(configuration.GetSection("Turnstile"));

        services.AddHttpClient<ITurnstileVerifier, CloudflareTurnstileVerifier>(http =>
        {
            http.BaseAddress = new Uri("https://challenges.cloudflare.com/");
            http.Timeout = TimeSpan.FromSeconds(5);
        });

        return services;
    }
}
