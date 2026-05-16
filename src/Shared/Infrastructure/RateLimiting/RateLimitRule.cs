namespace EduCrm.Infrastructure.RateLimiting;

public sealed class RateLimitRule
{
    public string Action { get; init; } = string.Empty;
    public int Limit { get; init; }
    public int WindowSeconds { get; init; }
}
