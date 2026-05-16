namespace EduCrm.Infrastructure.RateLimiting;

public interface IRateLimitCounterRepository
{
    Task<int> AcquireAsync(
        string action,
        string key,
        DateTime windowStartUtc,
        DateTime windowEndUtc,
        DateTime nowUtc,
        CancellationToken ct);
}
