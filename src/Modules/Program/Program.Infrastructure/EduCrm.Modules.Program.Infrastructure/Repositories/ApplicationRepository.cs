using ApplicationEntity = EduCrm.Modules.Program.Domain.Entities.Application;
using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Application.UseCases.ListApplications;
using EduCrm.Modules.Program.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Program.Infrastructure.Repositories;

public sealed class ApplicationRepository(AppDbContext db) : IApplicationRepository
{
    public void Add(ApplicationEntity application)
    {
        db.Applications.Add(application);
    }

    public Task<IReadOnlyList<ApplicationEntity>> GetActiveApplicationsByContactAsync(
        Guid programId,
        Guid organizationId,
        string normalizedEmail,
        string normalizedPhone,
        CancellationToken ct)
    {
        return db.Applications
            .Where(a => a.ProgramId == programId
                     && a.OrganizationId == organizationId
                     && (a.Status == ApplicationStatus.New || a.Status == ApplicationStatus.Contacted)
                     && (EF.Functions.ILike(a.SubmittedEmail, normalizedEmail)
                      || a.SubmittedPhone == normalizedPhone))
            .ToListAsync(ct)
            .ContinueWith<IReadOnlyList<ApplicationEntity>>(t => t.Result, ct);
    }

    public Task<ApplicationDetail?> GetDetailAsync(Guid applicationId, Guid organizationId, CancellationToken ct)
    {
        return (
            from a in db.Applications
            where a.Id == applicationId && a.OrganizationId == organizationId
            join p in db.Programs on a.ProgramId equals p.Id
            from pe in db.Persons
                .Where(x => x.Id == a.PersonId && x.OrganizationId == organizationId)
                .DefaultIfEmpty()
            select new ApplicationDetail(
                a.Id,
                a.Status,
                a.SubmittedFullName,
                a.SubmittedPhone,
                a.SubmittedMessage,
                a.FirstSubmittedAtUtc,
                a.LastSubmittedAtUtc,
                a.SubmissionCount,
                a.ConvertedAtUtc,
                a.ClosedAtUtc,
                a.ClosedNote,
                pe == null ? (Guid?)null : pe.Id,
                pe == null ? null : pe.FullName,
                p.Id,
                p.Name))
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);
    }

    public Task<ApplicationEntity?> GetTrackedByIdAsync(Guid applicationId, Guid organizationId, CancellationToken ct)
    {
        return db.Applications
            .FirstOrDefaultAsync(a => a.Id == applicationId && a.OrganizationId == organizationId, ct);
    }

    public async Task<ApplicationPagedListResult> GetPagedListAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken ct,
        IReadOnlyList<ApplicationStatus>? statuses = null,
        Guid? programId = null)
    {
        var baseQuery = db.Applications
            .AsNoTracking()
            .Where(a => a.OrganizationId == organizationId);

        if (programId.HasValue)
            baseQuery = baseQuery.Where(a => a.ProgramId == programId.Value);

        var filteredQuery = statuses is { Count: > 0 }
            ? baseQuery.Where(a => statuses.Contains(a.Status))
            : baseQuery;

        var totalCount = await filteredQuery.CountAsync(ct);

        var items = await (
            from a in filteredQuery
            join p in db.Programs on a.ProgramId equals p.Id
            orderby a.LastSubmittedAtUtc descending
            select new ApplicationListItem(
                a.Id,
                a.ProgramId,
                a.SubmittedFullName,
                a.SubmittedPhone,
                a.SubmittedEmail,
                p.Name,
                a.Status,
                a.LastSubmittedAtUtc,
                a.SubmissionCount))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ApplicationPagedListResult(items, totalCount);
    }

    public Task<int> CountNewAsync(Guid organizationId, CancellationToken ct)
    {
        return db.Applications
            .CountAsync(a => a.OrganizationId == organizationId && a.Status == ApplicationStatus.New, ct);
    }
}
