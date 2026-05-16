namespace EduCrm.Infrastructure.RateLimiting;

public interface IRateLimiter
{
    Task<RateLimitDecision> AcquireAsync(string action, string key, CancellationToken ct);
}
