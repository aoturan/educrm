using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduCrm.Infrastructure.RateLimiting;

[Table("rate_limit_counters")]
public sealed class RateLimitCounter
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("action")]
    public string Action { get; private set; } = null!;

    [Required]
    [Column("key")]
    public string Key { get; private set; } = null!;

    [Required]
    [Column("window_start_utc")]
    public DateTime WindowStartUtc { get; private set; }

    [Required]
    [Column("window_end_utc")]
    public DateTime WindowEndUtc { get; private set; }

    [Required]
    [Column("request_count")]
    public int RequestCount { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    [Required]
    [Column("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; private set; }

    private RateLimitCounter() { } // EF Core

    public RateLimitCounter(
        string action,
        string key,
        DateTime windowStartUtc,
        DateTime windowEndUtc,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action is required.", nameof(action));

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key is required.", nameof(key));

        if (windowEndUtc <= windowStartUtc)
            throw new ArgumentException("Window end must be after start.", nameof(windowEndUtc));

        Id = Guid.NewGuid();
        Action = action;
        Key = key;
        WindowStartUtc = windowStartUtc;
        WindowEndUtc = windowEndUtc;
        RequestCount = 1;
        CreatedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public void Increment(DateTime nowUtc)
    {
        RequestCount++;
        UpdatedAtUtc = nowUtc;
    }

    public void ResetWindow(DateTime windowStartUtc, DateTime windowEndUtc, DateTime nowUtc)
    {
        if (windowEndUtc <= windowStartUtc)
            throw new ArgumentException("Window end must be after start.", nameof(windowEndUtc));

        WindowStartUtc = windowStartUtc;
        WindowEndUtc = windowEndUtc;
        RequestCount = 1;
        UpdatedAtUtc = nowUtc;
    }
}
