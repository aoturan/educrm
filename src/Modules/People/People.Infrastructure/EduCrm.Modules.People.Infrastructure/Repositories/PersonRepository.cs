using EduCrm.Infrastructure.Data;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Domain.Entities;
using EduCrm.Modules.Program.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.People.Infrastructure.Repositories;

public sealed class PersonRepository : IPersonRepository
{
    private readonly AppDbContext _db;

    public PersonRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(Person person)
    {
        _db.Persons.Add(person);
    }

    public async Task<Person?> GetByIdAsync(Guid personId, Guid organizationId, CancellationToken ct)
    {
        return await _db.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == personId && p.OrganizationId == organizationId, ct);
    }

    public async Task<Person?> GetTrackedByIdAsync(Guid personId, Guid organizationId, CancellationToken ct)
    {
        return await _db.Persons
            .FirstOrDefaultAsync(p => p.Id == personId && p.OrganizationId == organizationId, ct);
    }

    public async Task<IReadOnlyList<PersonEnrolledProgramData>> GetEnrolledProgramsAsync(
        Guid personId,
        Guid organizationId,
        CancellationToken ct)
    {
        return await (
            from e in _db.Enrollments
            where e.PersonId == personId && e.OrganizationId == organizationId
            join p in _db.Programs on e.ProgramId equals p.Id
            orderby p.StartDate descending
            select new PersonEnrolledProgramData(
                p.Id,
                p.Name,
                p.StartDate,
                p.EndDate,
                p.Status))
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<PersonFollowUpData>> GetFollowUpsAsync(
        Guid personId,
        Guid organizationId,
        CancellationToken ct)
    {
        return await (
            from f in _db.FollowUps
            where f.PersonId == personId && f.OrganizationId == organizationId
            from prog in _db.Programs.Where(p => p.Id == f.ProgramId).DefaultIfEmpty()
            orderby f.DueAtUtc ascending
            select new PersonFollowUpData(
                f.Title,
                f.Status,
                f.Type,
                f.DueAtUtc,
                f.SnoozedUntilUtc,
                prog == null ? null : prog.Name))
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<(IReadOnlyList<PersonListItemData> Items, int TotalCount, int EnrolledCount, int NotEnrolledCount)> GetPagedListAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken ct,
        string? searchTerm = null,
        string? preFilter = null,
        bool showArchived = false)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var countQuery = _db.Persons
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId
                && (showArchived || !p.IsArchived)
                && (searchTerm == null
                    || EF.Functions.ILike(p.FullName, $"%{searchTerm}%")
                    || (p.Email != null && EF.Functions.ILike(p.Email, $"%{searchTerm}%"))
                    || (p.Phone != null && EF.Functions.ILike(p.Phone, $"%{searchTerm}%"))));

        var baseQuery = countQuery
            .Where(p => preFilter == null
                || (preFilter == "enrolled"
                    && _db.Enrollments.Any(e => e.PersonId == p.Id && e.OrganizationId == organizationId))
                || (preFilter == "not-enrolled"
                    && !_db.Enrollments.Any(e => e.PersonId == p.Id && e.OrganizationId == organizationId)));

        var totalCount = await countQuery.CountAsync(ct);

        var enrolledCount = await countQuery
            .CountAsync(p => _db.Enrollments
                .Any(e => e.PersonId == p.Id && e.OrganizationId == organizationId), ct);

        var notEnrolledCount = totalCount - enrolledCount;

        var items = await baseQuery
            .OrderByDescending(p => p.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PersonListItemData(
                p.Id,
                p.FullName,
                p.Email,
                p.Phone,
                _db.Enrollments.Count(e => e.PersonId == p.Id && e.OrganizationId == organizationId),
                _db.Enrollments
                    .Join(_db.Programs, e => e.ProgramId, pr => pr.Id, (e, pr) => new { e, pr })
                    .Any(x => x.e.PersonId == p.Id
                              && x.e.OrganizationId == organizationId
                              && x.pr.Status == ProgramStatus.Active
                              && x.pr.EndDate >= today),
                p.IsArchived))
            .ToListAsync(ct);

        return (items, totalCount, enrolledCount, notEnrolledCount);
    }
}