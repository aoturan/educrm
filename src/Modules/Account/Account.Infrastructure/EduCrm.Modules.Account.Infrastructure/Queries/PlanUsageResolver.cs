using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Account.Contracts.Dtos;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EduCrm.Modules.Account.Infrastructure.Queries;

public sealed class PlanUsageResolver : IPlanUsageResolver
{
    private readonly AppDbContext _db;
    private readonly IOptionsSnapshot<PlanLimitsOptions> _options;
    private readonly IClock _clock;

    public PlanUsageResolver(AppDbContext db, IOptionsSnapshot<PlanLimitsOptions> options, IClock clock)
    {
        _db = db;
        _options = options;
        _clock = clock;
    }

    public async Task<PlanUsageSnapshot> ResolveAsync(Guid organizationId, CancellationToken ct)
    {
        var subscription = await _db.Subscriptions
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .Select(s => new
            {
                s.PlanCode,
                s.StartsAtUtc,
                s.EndsAtUtc,
                s.DowngradedFromPlanCode,
                s.DowngradedAtUtc
            })
            .FirstOrDefaultAsync(ct);

        if (subscription is null)
            throw new InvalidOperationException(
                $"Organization {organizationId} has no subscription record. RegisterService should have created one.");

        var now = _clock.UtcNow.UtcDateTime;
        var isExpired = subscription.EndsAtUtc is { } endsAt && endsAt < now;

        var limits = isExpired
            ? _options.Value.Free
            : subscription.PlanCode switch
            {
                PlanCode.Plus => _options.Value.Plus,
                PlanCode.Pro  => _options.Value.Pro,
                _             => _options.Value.Free,
            };

        var statusString = isExpired ? "Expired" : "Active";

        // In-progress request = anything not in a terminal state.
        // Excluding terminal states (Approved/Rejected/Cancelled) auto-includes any
        // new non-terminal status added later without touching this query.
        var pendingRequest = await _db.SubscriptionRequests
            .AsNoTracking()
            .Where(r => r.OrganizationId == organizationId
                     && r.Status != RequestStatus.Approved
                     && r.Status != RequestStatus.Rejected
                     && r.Status != RequestStatus.Cancelled)
            .OrderByDescending(r => r.RequestedAtUtc)
            .Select(r => new PendingSubscriptionRequestData(
                r.RequestedPlanCode.ToString(),
                r.Status.ToString(),
                r.PaymentMethod.ToString(),
                r.Amount,
                r.PaymentReferenceCode,
                r.RequestedAtUtc,
                _db.SubscriptionPaymentNotifications.Any(n => n.SubscriptionRequestId == r.Id)))
            .FirstOrDefaultAsync(ct);

        return new PlanUsageSnapshot(
            subscription.PlanCode.ToString(),
            statusString,
            subscription.StartsAtUtc,
            subscription.EndsAtUtc,
            subscription.DowngradedFromPlanCode?.ToString(),
            subscription.DowngradedAtUtc,
            limits,
            pendingRequest);
    }
}