using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Repositories;

public sealed class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _db;

    public SubscriptionRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(Subscription subscription)
    {
        _db.Subscriptions.Add(subscription);
    }

    public Task<Subscription?> GetCurrentByOrganizationAsync(Guid organizationId, CancellationToken ct)
    {
        return _db.Subscriptions
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .FirstOrDefaultAsync(ct);
    }

    public Task<Subscription?> GetCurrentTrackedByOrganizationAsync(Guid organizationId, CancellationToken ct)
    {
        return _db.Subscriptions
            .Where(s => s.OrganizationId == organizationId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .FirstOrDefaultAsync(ct);
    }

    public Task<int> CountActivePaidAsync(DateTime nowUtc, CancellationToken ct)
    {
        return _db.Subscriptions
            .AsNoTracking()
            .CountAsync(s => s.PlanCode != PlanCode.Free
                          && (s.EndsAtUtc == null || s.EndsAtUtc > nowUtc), ct);
    }

    public Task<int> CountFreeOrExpiredAsync(DateTime nowUtc, CancellationToken ct)
    {
        return _db.Subscriptions
            .AsNoTracking()
            .CountAsync(s => s.PlanCode == PlanCode.Free
                          || (s.EndsAtUtc != null && s.EndsAtUtc <= nowUtc), ct);
    }
}