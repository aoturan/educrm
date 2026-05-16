using EduCrm.SharedKernel.Abstractions;
using Microsoft.Extensions.Options;

namespace EduCrm.Infrastructure.RateLimiting;

public sealed class RateLimiter : IRateLimiter
{
    private readonly IRateLimitCounterRepository _repo;
    private readonly IClock _clock;
    private readonly RateLimitOptions _options;

    public RateLimiter(
        IRateLimitCounterRepository repo,
        IClock clock,
        IOptions<RateLimitOptions> options)
    {
        _repo = repo;
        _clock = clock;
        _options = options.Value;
    }

    public async Task<RateLimitDecision> AcquireAsync(string action, string key, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action is required.", nameof(action));
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key is required.", nameof(key));

        var rule = FindRule(action)
                   ?? throw new InvalidOperationException(
                       $"RateLimit rule not configured for action '{action}'.");

        var now = _clock.UtcNow.UtcDateTime;
        var windowTicks = TimeSpan.FromSeconds(rule.WindowSeconds).Ticks;
        var windowStart = new DateTime((now.Ticks / windowTicks) * windowTicks, DateTimeKind.Utc);
        var windowEnd = windowStart.AddSeconds(rule.WindowSeconds);

        var count = await _repo.AcquireAsync(action, key, windowStart, windowEnd, now, ct);

        return new RateLimitDecision(
            IsAllowed: count <= rule.Limit,
            Action: action,
            Limit: rule.Limit,
            CurrentCount: count);
    }

    private RateLimitRule? FindRule(string action) =>
        _options.Rules.FirstOrDefault(r =>
            string.Equals(r.Action, action, StringComparison.OrdinalIgnoreCase));
}
