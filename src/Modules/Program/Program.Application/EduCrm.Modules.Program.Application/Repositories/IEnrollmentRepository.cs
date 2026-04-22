using EduCrm.Modules.Program.Domain.Entities;
using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.Repositories;

public record EnrollmentCandidateData(
    Guid PersonId,
    string FullName,
    string? Phone,
    string? Email);

public interface IEnrollmentRepository
{
    void Add(Enrollment enrollment);
    Task<bool> DeleteAsync(Guid enrollmentId, Guid organizationId, CancellationToken ct);
    Task<bool> ExistsAsync(Guid programId, Guid personId, Guid organizationId, CancellationToken ct);
    Task<Guid?> GetIdAsync(Guid programId, Guid personId, Guid organizationId, CancellationToken ct);
    Task<bool> PersonExistsInOrgAsync(Guid personId, Guid organizationId, CancellationToken ct);
    Task<ProgramStatus?> GetProgramStatusByEnrollmentIdAsync(Guid enrollmentId, Guid organizationId, CancellationToken ct);
    Task<(IReadOnlyList<EnrollmentCandidateData> items, int totalCount)> GetCandidatesAsync(
        Guid programId,
        Guid organizationId,
        string? search,
        int page,
        int pageSize,
        CancellationToken ct);
}

