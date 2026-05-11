using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Queries;

public sealed class ExportRateLimiter : IExportRateLimiter
{
    private static readonly TimeSpan Window = TimeSpan.FromSeconds(30);

    private readonly AppDbContext _db;
    private readonly IClock _clock;

    public ExportRateLimiter(AppDbContext db, IClock clock)
    {
        _db = db;
        _clock = clock;
    }

    public async Task<bool> TryReserveSlotAsync(Guid userId, CancellationToken ct)
    {
        var now = _clock.UtcNow.UtcDateTime;
        var cutoff = now - Window;

        var affected = await _db.Users
            .Where(u => u.Id == userId
                        && (u.LastExportedAtUtc == null || u.LastExportedAtUtc < cutoff))
            .ExecuteUpdateAsync(
                s => s.SetProperty(u => u.LastExportedAtUtc, now),
                ct);

        return affected == 1;
    }
}
