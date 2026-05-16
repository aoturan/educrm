namespace EduCrm.Infrastructure.RateLimiting;

public sealed record RateLimitDecision(
    bool IsAllowed,
    string Action,
    int Limit,
    int CurrentCount);
