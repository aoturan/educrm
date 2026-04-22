using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Application;

public sealed record ApplicationListItemResponse(
    Guid Id,
    Guid ProgramId,
    string? SubmittedFullName,
    string? SubmittedPhone,
    string? SubmittedEmail,
    string ProgramName,
    ApplicationStatus Status,
    DateTime LastSubmittedAtUtc,
    int SubmissionCount);

public sealed record ApplicationListResponse(
    IReadOnlyList<ApplicationListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);

