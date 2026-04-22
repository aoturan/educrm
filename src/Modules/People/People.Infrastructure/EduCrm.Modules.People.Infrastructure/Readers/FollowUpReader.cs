using EduCrm.Infrastructure.Data;
using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.People.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.People.Infrastructure.Readers;

public sealed class FollowUpReader(AppDbContext db) : IFollowUpReader
{
    public Task<int> CountOpenAsync(Guid organizationId, CancellationToken ct)
    {
        return db.FollowUps
            .CountAsync(f => f.OrganizationId == organizationId && f.Status == FollowUpStatus.Open, ct);
    }

    public Task<int> CountOpenAndOverdueAsync(Guid organizationId, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return db.FollowUps
            .CountAsync(f => f.OrganizationId == organizationId
                          && f.Status == FollowUpStatus.Open
                          && f.DueAtUtc < now, ct);
    }
}

