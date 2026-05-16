namespace EduCrm.Infrastructure.RateLimiting;

public sealed class RateLimitOptions
{
    public IReadOnlyList<RateLimitRule> Rules { get; init; } = Array.Empty<RateLimitRule>();
}
