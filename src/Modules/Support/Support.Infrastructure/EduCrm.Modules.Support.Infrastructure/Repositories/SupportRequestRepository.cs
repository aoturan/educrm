using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.Modules.Support.Application.Repositories.Models;
using EduCrm.Modules.Support.Domain.Entities;
using EduCrm.Modules.Support.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Support.Infrastructure.Repositories;

public sealed class SupportRequestRepository(AppDbContext db) : ISupportRequestRepository
{
    public void Add(SupportRequest supportRequest)
    {
        db.SupportRequests.Add(supportRequest);
    }

    public Task<SupportRequest?> GetTrackedByIdAsync(Guid id, CancellationToken ct)
    {
        return db.SupportRequests.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<SupportRequestPagedListQueryResult> GetPagedListAsync(
        int page,
        int pageSize,
        CancellationToken ct,
        string? preFilter = null)
    {
        var baseQuery =
            from r in db.SupportRequests.AsNoTracking()
            join o in db.Organizations.AsNoTracking() on r.OrganizationId equals o.Id
            select new { Request = r, OrganizationName = o.Name };

        var filteredQuery = preFilter switch
        {
            "new" => baseQuery.Where(x => x.Request.Status == SupportRequestStatus.New),
            "handled" => baseQuery.Where(x => x.Request.Status == SupportRequestStatus.Handled),
            _ => baseQuery
        };

        var totalCount = await filteredQuery.CountAsync(ct);

        var items = await filteredQuery
            .OrderByDescending(x => x.Request.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SupportRequestListItem(
                x.Request.Id,
                x.Request.OrganizationId,
                x.OrganizationName,
                x.Request.PageUrl,
                x.Request.Subject,
                x.Request.Message,
                x.Request.Status,
                x.Request.CreatedAtUtc))
            .ToListAsync(ct);

        return new SupportRequestPagedListQueryResult(items, totalCount);
    }
}
