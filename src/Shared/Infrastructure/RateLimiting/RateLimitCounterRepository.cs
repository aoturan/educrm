using EduCrm.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Infrastructure.RateLimiting;

public sealed class RateLimitCounterRepository : IRateLimitCounterRepository
{
    private readonly AppDbContext _db;

    public RateLimitCounterRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int> AcquireAsync(
        string action,
        string key,
        DateTime windowStartUtc,
        DateTime windowEndUtc,
        DateTime nowUtc,
        CancellationToken ct)
    {
        var rows = await _db.Database
            .SqlQuery<int>($"""
                INSERT INTO rate_limit_counters
                  (action, key, window_start_utc, window_end_utc, request_count, created_at_utc, updated_at_utc)
                VALUES ({action}, {key}, {windowStartUtc}, {windowEndUtc}, 1, {nowUtc}, {nowUtc})
                ON CONFLICT (action, key, window_start_utc) DO UPDATE
                  SET request_count = rate_limit_counters.request_count + 1,
                      updated_at_utc = EXCLUDED.updated_at_utc
                RETURNING request_count
                """)
            .ToListAsync(ct);

        return rows.Single();
    }
}
