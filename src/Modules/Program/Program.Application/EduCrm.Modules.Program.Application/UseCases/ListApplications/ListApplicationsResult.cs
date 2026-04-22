using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.ListApplications;

public sealed record ApplicationListItem(
    Guid Id,
    Guid ProgramId,
    string? SubmittedFullName,
    string? SubmittedPhone,
    string? SubmittedEmail,
    string ProgramName,
    ApplicationStatus Status,
    DateTime LastSubmittedAtUtc,
    int SubmissionCount);

public sealed record ListApplicationsResult(
    IReadOnlyList<ApplicationListItem> Items,
    int Page,
    int PageSize,
    int TotalCount);

