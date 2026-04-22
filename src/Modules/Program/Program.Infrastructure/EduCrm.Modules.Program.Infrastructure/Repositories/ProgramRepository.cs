using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Program.Infrastructure.Repositories;

public class ProgramRepository: IProgramRepository
{
    private readonly AppDbContext _db;

    public ProgramRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(Domain.Entities.Program program)
    {
        _db.Programs.Add(program);
    }

    public async Task<IReadOnlyList<Domain.Entities.Program>> GetActiveByOrganizationIdAsync(Guid organizationId, CancellationToken ct)
    {
        return await _db.Programs
            .AsNoTracking()
            .Where(x => x.OrganizationId == organizationId && x.Status == ProgramStatus.Active)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<ProgramListItemData>> GetAllByOrganizationIdAsync(
        Guid organizationId,
        CancellationToken ct,
        string? nameQuery = null)
    {
        var query =
            from p in _db.Programs
            where p.OrganizationId == organizationId
               && (nameQuery == null || EF.Functions.ILike(p.Name, $"%{nameQuery}%"))
            orderby p.CreatedAtUtc descending
            select new ProgramListItemData(
                p.Id,
                p.Name,
                p.PublicShortDescription,
                p.Status,
                p.StartDate,
                p.EndDate,
                _db.Enrollments.Count(e => e.ProgramId == p.Id && e.OrganizationId == organizationId),
                p.IsArchived);

        return await query.AsNoTracking().ToListAsync(ct);
    }

    public async Task<ProgramPagedListQueryResult> GetPagedListAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken ct,
        string? searchTerm = null,
        string? preFilter = null,
        bool showArchived = false,
        Guid? personId = null,
        bool onlyApproaching = false)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var in7Days = today.AddDays(7);

        var baseQuery = _db.Programs
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId
                && (showArchived || !p.IsArchived)
                && (searchTerm == null || EF.Functions.ILike(p.Name, $"%{searchTerm}%"))
                && (personId == null || _db.Enrollments.Any(e => e.ProgramId == p.Id && e.PersonId == personId && e.OrganizationId == organizationId))
                && (!onlyApproaching || (p.StartDate >= today && p.StartDate <= in7Days)));

        var filteredQuery = preFilter switch
        {
            "active"      => baseQuery.Where(p => p.Status == ProgramStatus.Active),
            "completed"   => baseQuery.Where(p => p.Status == ProgramStatus.Completed),
            "not-started" => baseQuery.Where(p => p.Status == ProgramStatus.Active && p.StartDate > today),
            "ongoing"     => baseQuery.Where(p => p.Status == ProgramStatus.Active && p.StartDate <= today && p.EndDate >= today),
            _             => baseQuery
        };

        var totalCount = await filteredQuery.CountAsync(ct);

        var items = await filteredQuery
            .OrderByDescending(p => p.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProgramListItemData(
                p.Id,
                p.Name,
                p.PublicShortDescription,
                p.Status,
                p.StartDate,
                p.EndDate,
                _db.Enrollments.Count(e => e.ProgramId == p.Id && e.OrganizationId == organizationId),
                p.IsArchived))
            .ToListAsync(ct);

        return new ProgramPagedListQueryResult(items, totalCount);
    }

    public async Task<(Domain.Entities.Program program, IReadOnlyList<EnrollmentWithPersonData> enrollments)?> GetByIdAsync(
        Guid programId,
        Guid organizationId,
        CancellationToken ct)
    {
        var program = await _db.Programs
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == programId && p.OrganizationId == organizationId, ct);

        if (program is null)
            return null;

        var enrollments = await (
            from e in _db.Enrollments
            where e.ProgramId == programId && e.OrganizationId == organizationId
            join p in _db.Persons on e.PersonId equals p.Id
             orderby e.EnrolledAtUtc descending
            select new EnrollmentWithPersonData(
                e.Id,
                e.PersonId,
                e.EnrolledAtUtc,
                p.FullName,
                p.Email,
                p.Phone))
            .AsNoTracking()
            .ToListAsync(ct);

        return (program, enrollments);
    }

    public async Task<Domain.Entities.Program?> GetTrackedByIdAsync(
        Guid programId,
        Guid organizationId,
        CancellationToken ct)
    {
        return await _db.Programs
            .FirstOrDefaultAsync(p => p.Id == programId && p.OrganizationId == organizationId, ct);
    }

    public async Task<(Domain.Enums.ProgramStatus Status, DateOnly EndDate)?> GetEnrollmentCheckAsync(
        Guid programId,
        Guid organizationId,
        CancellationToken ct)
    {
        return await _db.Programs
            .AsNoTracking()
            .Where(p => p.Id == programId && p.OrganizationId == organizationId)
            .Select(p => ((Domain.Enums.ProgramStatus Status, DateOnly EndDate)?)
                new(p.Status, p.EndDate))
            .FirstOrDefaultAsync(ct);
    }

    public Task<bool> ExistsAsync(Guid programId, Guid organizationId, CancellationToken ct)
    {
        return _db.Programs
            .AnyAsync(p => p.Id == programId && p.OrganizationId == organizationId, ct);
    }

    public Task<Guid?> GetOrganizationIdAsync(Guid programId, CancellationToken ct)
    {
        return _db.Programs
            .Where(p => p.Id == programId)
            .Select(p => (Guid?)p.OrganizationId)
            .FirstOrDefaultAsync(ct);
    }

    public Task<(Guid OrganizationId, ProgramStatus Status, bool IsPublic)?> GetPublicApplicationCheckAsync(Guid programId, CancellationToken ct)
    {
        return _db.Programs
            .Where(p => p.Id == programId)
            .Select(p => ((Guid OrganizationId, ProgramStatus Status, bool IsPublic)?)
                new ValueTuple<Guid, ProgramStatus, bool>(p.OrganizationId, p.Status, p.IsPublic))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PublicApplicationCheckData?> GetPublicApplicationCheckBySlugAsync(string slug, CancellationToken ct)
    {
        var row = await _db.Programs
            .Where(p => p.PublicSlug == slug)
            .Select(p => new { p.Id, p.OrganizationId, p.Status, p.IsPublic })
            .FirstOrDefaultAsync(ct);

        if (row is null) return null;

        return new PublicApplicationCheckData(row.Id, row.OrganizationId, row.Status, row.IsPublic);
    }

    public Task<PublicProgramData?> GetPublicBySlugAsync(string slug, CancellationToken ct)    {
        return (
            from p in _db.Programs
            where p.PublicSlug == slug && p.IsPublic
            join o in _db.Organizations on p.OrganizationId equals o.Id
            select new PublicProgramData(
                p.Name,
                p.StartDate,
                p.EndDate,
                p.Capacity,
                p.PublicShortDescription,
                p.PublicDetailedDescription,
                p.PublicModality,
                p.PublicScheduleText,
                p.PublicInstructorName,
                p.PublicEnrollmentDeadline,
                p.LocationDetails,
                p.OnlineParticipationInfo,
                p.PriceAmount,
                p.PriceCurrency,
                p.PriceNote,
                p.PriceType,
                p.IsPublic,
                o.Name))
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);
    }

    public Task<int> CountActiveStartingInNext7DaysAsync(Guid organizationId, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var in7Days = today.AddDays(7);

        return _db.Programs
            .CountAsync(p => p.OrganizationId == organizationId
                          && !p.IsArchived
                          && p.Status == ProgramStatus.Active
                          && p.StartDate >= today
                          && p.StartDate < in7Days, ct);
    }
}