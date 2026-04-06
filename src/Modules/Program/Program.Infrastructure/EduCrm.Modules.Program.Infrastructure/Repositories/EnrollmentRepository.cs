using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Domain.Entities;
using EduCrm.Modules.Program.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Program.Infrastructure.Repositories;

public sealed class EnrollmentRepository(AppDbContext db) : IEnrollmentRepository
{
    public void Add(Enrollment enrollment)
    {
        db.Enrollments.Add(enrollment);
    }

    public async Task<bool> DeleteAsync(Guid enrollmentId, Guid organizationId, CancellationToken ct)
    {
        var rows = await db.Enrollments
            .Where(e => e.Id == enrollmentId && e.OrganizationId == organizationId)
            .ExecuteDeleteAsync(ct);

        return rows > 0;
    }

    public async Task<(IReadOnlyList<EnrollmentCandidateData> items, int totalCount)> GetCandidatesAsync(
        Guid programId,
        Guid organizationId,
        string? search,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var enrolledPersonIds = db.Enrollments
            .Where(e => e.ProgramId == programId && e.OrganizationId == organizationId)
            .Select(e => e.PersonId);

        var query =
            from p in db.Persons
            where p.OrganizationId == organizationId
               && !p.IsArchived
               && !enrolledPersonIds.Contains(p.Id)
               && (search == null
                   || EF.Functions.ILike(p.FullName, $"%{search}%")
                   || (p.Email != null && EF.Functions.ILike(p.Email, $"%{search}%"))
                   || (p.Phone != null && EF.Functions.ILike(p.Phone, $"%{search}%")))
            orderby p.FullName
            select new EnrollmentCandidateData(p.Id, p.FullName, p.Phone, p.Email);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public Task<bool> ExistsAsync(Guid programId, Guid personId, Guid organizationId, CancellationToken ct)
    {
        return db.Enrollments
            .AnyAsync(e => e.ProgramId == programId
                        && e.PersonId == personId
                        && e.OrganizationId == organizationId, ct);
    }

    public Task<bool> PersonExistsInOrgAsync(Guid personId, Guid organizationId, CancellationToken ct)
    {
        return db.Persons
            .AnyAsync(p => p.Id == personId && p.OrganizationId == organizationId, ct);
    }

    public async Task<ProgramStatus?> GetProgramStatusByEnrollmentIdAsync(
        Guid enrollmentId,
        Guid organizationId,
        CancellationToken ct)
    {
        return await (
            from e in db.Enrollments
            where e.Id == enrollmentId && e.OrganizationId == organizationId
            join p in db.Programs on e.ProgramId equals p.Id
            select (ProgramStatus?)p.Status)
            .FirstOrDefaultAsync(ct);
    }
}
