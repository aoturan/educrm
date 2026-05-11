using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Repositories.Models;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Repositories;

public sealed class OrganizationRepository : IOrganizationRepository
{
    private readonly AppDbContext _db;

    public OrganizationRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(Organization organization)
    {
        _db.Organizations.Add(organization);
    }

    public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Organizations
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public Task<int> CountCreatedSinceAsync(DateTime sinceUtc, CancellationToken ct)
    {
        return _db.Organizations
            .AsNoTracking()
            .CountAsync(o => o.CreatedAtUtc >= sinceUtc, ct);
    }

    public async Task<OrganizationPagedListQueryResult> GetPagedListAsync(
        int page,
        int pageSize,
        DateTime nowUtc,
        CancellationToken ct,
        string? searchTerm = null,
        string? face = null)
    {
        var weekAgo = nowUtc.AddDays(-7);

        var baseQuery = _db.Organizations
            .AsNoTracking()
            .Where(o => searchTerm == null
                || EF.Functions.ILike(o.Name, $"%{searchTerm}%")
                || EF.Functions.ILike(o.ContactName, $"%{searchTerm}%")
                || EF.Functions.ILike(o.ContactEmail, $"%{searchTerm}%"));

        var filteredQuery = face switch
        {
            "newOrganizations" => baseQuery
                .Where(o => o.CreatedAtUtc >= weekAgo),

            "activePaidOrganizations" => baseQuery
                .Where(o => _db.Subscriptions.Any(s =>
                    s.OrganizationId == o.Id
                    && s.PlanCode != PlanCode.Free
                    && (s.EndsAtUtc == null || s.EndsAtUtc > nowUtc))),

            "freeOrganizations" => baseQuery
                .Where(o => _db.Subscriptions.Any(s =>
                    s.OrganizationId == o.Id
                    && s.PlanCode == PlanCode.Free)),

            _ => baseQuery
        };

        var totalCount = await filteredQuery.CountAsync(ct);

        var items = await filteredQuery
            .OrderByDescending(o => o.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OrganizationListItem(
                o.Id,
                o.Name,
                o.ContactName,
                o.ContactEmail,
                o.ContactPhone,
                o.CreatedAtUtc))
            .ToListAsync(ct);

        return new OrganizationPagedListQueryResult(items, totalCount);
    }
}
