using EduCrm.Infrastructure.Data;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Domain.Entities;
using EduCrm.Modules.People.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.People.Infrastructure.Repositories;

public sealed class FollowUpRepository : IFollowUpRepository
{
    private readonly AppDbContext _db;

    public FollowUpRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(FollowUp followUp)
    {
        _db.FollowUps.Add(followUp);
    }

    public async Task<FollowUp?> GetTrackedByIdAsync(Guid followUpId, Guid organizationId, CancellationToken ct)
    {
        return await _db.FollowUps
            .FirstOrDefaultAsync(f => f.Id == followUpId && f.OrganizationId == organizationId, ct);
    }

    public async Task<FollowUpByIdData?> GetByIdAsync(Guid followUpId, Guid organizationId, CancellationToken ct)
    {
        var result = await (
            from f in _db.FollowUps
            where f.Id == followUpId && f.OrganizationId == organizationId
            join p in _db.Persons on f.PersonId equals p.Id
            from prog in _db.Programs.Where(x => x.Id == f.ProgramId).DefaultIfEmpty()
            select new FollowUpByIdData(
                f.Id,
                f.OrganizationId,
                f.Type,
                f.Status,
                f.Title,
                f.Note,
                f.DueAtUtc,
                f.SnoozedUntilUtc,
                f.CompletedAtUtc,
                f.CancelledAtUtc,
                new FollowUpPersonInfo(p.Id, p.FullName, p.Email, p.Phone),
                prog == null ? null : new FollowUpProgramInfo(prog.Id, prog.Name)))
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

        return result;
    }

    public async Task<(IReadOnlyList<FollowUpListItemData> Items, int TotalCount)> GetListAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken ct,
        IReadOnlyList<FollowUpType>? typeFilter = null,
        IReadOnlyList<FollowUpStatus>? statusFilter = null,
        Guid? personId = null,
        Guid? programId = null)
    {
        var query =
            from f in _db.FollowUps
            where f.OrganizationId == organizationId
               && (personId == null || f.PersonId == personId)
               && (programId == null || f.ProgramId == programId)
               && (typeFilter == null || typeFilter.Count == 0 || typeFilter.Contains(f.Type))
               && (statusFilter == null || statusFilter.Count == 0 || statusFilter.Contains(f.Status))
            join p in _db.Persons on f.PersonId equals p.Id
            from prog in _db.Programs.Where(x => x.Id == f.ProgramId).DefaultIfEmpty()
            orderby f.DueAtUtc ascending
            select new FollowUpListItemData(
                f.Id,
                p.FullName,
                prog == null ? null : prog.Name,
                f.Type,
                f.Status,
                f.Title,
                f.DueAtUtc,
                f.SnoozedUntilUtc);

        var totalCount = await query.AsNoTracking().CountAsync(ct);

        var items = await query
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
