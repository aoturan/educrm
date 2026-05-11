using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Repositories.Models;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Repositories;

public sealed class SubscriptionRequestRepository : ISubscriptionRequestRepository
{
    private readonly AppDbContext _db;

    public SubscriptionRequestRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(SubscriptionRequest request)
    {
        _db.SubscriptionRequests.Add(request);
    }

    public Task<SubscriptionRequest?> GetByIdAsync(Guid requestId, CancellationToken ct)
    {
        return _db.SubscriptionRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == requestId, ct);
    }

    public Task<SubscriptionRequest?> GetTrackedByIdAsync(Guid requestId, CancellationToken ct)
    {
        return _db.SubscriptionRequests
            .FirstOrDefaultAsync(r => r.Id == requestId, ct);
    }

    public Task<SubscriptionRequest?> GetActiveByOrganizationAsync(Guid organizationId, CancellationToken ct)
    {
        return _db.SubscriptionRequests
            .Where(r => r.OrganizationId == organizationId
                     && (r.Status == RequestStatus.PendingPayment
                      || r.Status == RequestStatus.PendingApproval))
            .OrderByDescending(r => r.RequestedAtUtc)
            .FirstOrDefaultAsync(ct);
    }

    public Task<bool> ExistsByReferenceCodeAsync(string referenceCode, CancellationToken ct)
    {
        return _db.SubscriptionRequests
            .AsNoTracking()
            .AnyAsync(r => r.PaymentReferenceCode == referenceCode, ct);
    }

    public Task<int> CountPendingAsync(CancellationToken ct)
    {
        return _db.SubscriptionRequests
            .AsNoTracking()
            .CountAsync(r => r.Status == RequestStatus.PendingPayment
                          || r.Status == RequestStatus.PendingApproval, ct);
    }

    public async Task<IReadOnlyList<PendingPaymentRequestSummary>> GetOldestPendingAsync(int take, CancellationToken ct)
    {
        var query =
            from r in _db.SubscriptionRequests.AsNoTracking()
            where r.Status == RequestStatus.PendingPayment
               || r.Status == RequestStatus.PendingApproval
            join o in _db.Organizations.AsNoTracking() on r.OrganizationId equals o.Id
            orderby r.RequestedAtUtc ascending
            select new PendingPaymentRequestSummary(
                r.Id,
                r.OrganizationId,
                o.Name,
                r.RequestedPlanCode.ToString(),
                r.Status.ToString(),
                r.Amount,
                r.PaymentReferenceCode,
                r.RequestedAtUtc,
                _db.SubscriptionPaymentNotifications.Any(n => n.SubscriptionRequestId == r.Id));

        return await query.Take(take).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<OrganizationSubscriptionRequestData>> GetByOrganizationAsync(Guid organizationId, CancellationToken ct)
    {
        return await _db.SubscriptionRequests
            .AsNoTracking()
            .Where(r => r.OrganizationId == organizationId)
            .OrderByDescending(r => r.RequestedAtUtc)
            .Select(r => new OrganizationSubscriptionRequestData(
                r.Id,
                r.RequestedPlanCode,
                r.Status,
                r.PaymentMethod,
                r.Amount,
                r.PaymentReferenceCode,
                r.RequestedAtUtc,
                r.ExpiresAtUtc,
                r.ApprovedAtUtc,
                r.RejectedAtUtc,
                r.CancelledAtUtc,
                r.IsInvoiced,
                _db.SubscriptionPaymentNotifications.Any(n => n.SubscriptionRequestId == r.Id)))
            .ToListAsync(ct);
    }

    public async Task<SubscriptionRequestPagedListQueryResult> GetPagedListAsync(
        int page,
        int pageSize,
        CancellationToken ct,
        string? searchTerm = null,
        string? face = null)
    {
        var baseQuery =
            from r in _db.SubscriptionRequests.AsNoTracking()
            join o in _db.Organizations.AsNoTracking() on r.OrganizationId equals o.Id
            where searchTerm == null
                  || EF.Functions.ILike(r.PaymentReferenceCode, $"%{searchTerm}%")
                  || EF.Functions.ILike(o.Name, $"%{searchTerm}%")
            select new { Request = r, OrganizationName = o.Name };

        var filteredQuery = face switch
        {
            "pending" => baseQuery.Where(x =>
                x.Request.Status == RequestStatus.PendingPayment
                || x.Request.Status == RequestStatus.PendingApproval),
            "approved" => baseQuery.Where(x => x.Request.Status == RequestStatus.Approved),
            "rejected" => baseQuery.Where(x => x.Request.Status == RequestStatus.Rejected),
            "cancelled" => baseQuery.Where(x => x.Request.Status == RequestStatus.Cancelled),
            _ => baseQuery
        };

        var totalCount = await filteredQuery.CountAsync(ct);

        var items = await filteredQuery
            .OrderByDescending(x => x.Request.RequestedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SubscriptionRequestListItem(
                x.Request.Id,
                x.Request.OrganizationId,
                x.OrganizationName,
                x.Request.RequestedPlanCode,
                x.Request.Status,
                x.Request.PaymentMethod,
                x.Request.Amount,
                x.Request.PaymentReferenceCode,
                x.Request.RequestedAtUtc,
                x.Request.ExpiresAtUtc,
                x.Request.ApprovedAtUtc,
                x.Request.RejectedAtUtc,
                x.Request.CancelledAtUtc,
                x.Request.IsInvoiced,
                _db.SubscriptionPaymentNotifications.Any(n => n.SubscriptionRequestId == x.Request.Id)))
            .ToListAsync(ct);

        return new SubscriptionRequestPagedListQueryResult(items, totalCount);
    }
}
