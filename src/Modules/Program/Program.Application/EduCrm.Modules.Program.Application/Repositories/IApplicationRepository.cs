using ApplicationEntity = EduCrm.Modules.Program.Domain.Entities.Application;
using EduCrm.Modules.Program.Application.UseCases.GetApplicationById;
using EduCrm.Modules.Program.Application.UseCases.ListApplications;
using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.Repositories;

public sealed record ApplicationDetail(
    Guid Id,
    ApplicationStatus Status,
    string SubmittedFullName,
    string SubmittedPhone,
    string? SubmittedMessage,
    DateTime FirstSubmittedAtUtc,
    DateTime LastSubmittedAtUtc,
    int SubmissionCount,
    DateTime? ConvertedAtUtc,
    DateTime? ClosedAtUtc,
    string? ClosedNote,
    Guid? PersonId,
    string? PersonFullName,
    Guid ProgramId,
    string ProgramName);

public record ApplicationPagedListResult(
    IReadOnlyList<ApplicationListItem> Items,
    int TotalCount);

public interface IApplicationRepository
{
    void Add(ApplicationEntity application);
    Task<IReadOnlyList<ApplicationEntity>> GetActiveApplicationsByContactAsync(Guid programId, Guid organizationId, string normalizedEmail, string normalizedPhone, CancellationToken ct);
    Task<ApplicationDetail?> GetDetailAsync(Guid applicationId, Guid organizationId, CancellationToken ct);
    Task<ApplicationPagedListResult> GetPagedListAsync(Guid organizationId, int page, int pageSize, CancellationToken ct, IReadOnlyList<ApplicationStatus>? statuses = null, Guid? programId = null);
    Task<ApplicationEntity?> GetTrackedByIdAsync(Guid applicationId, Guid organizationId, CancellationToken ct);
    Task<int> CountNewAsync(Guid organizationId, CancellationToken ct);
}
