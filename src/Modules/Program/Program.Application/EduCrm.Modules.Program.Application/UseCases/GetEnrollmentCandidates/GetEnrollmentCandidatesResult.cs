namespace EduCrm.Modules.Program.Application.UseCases.GetEnrollmentCandidates;

public sealed record EnrollmentCandidateItem(
    Guid Id,
    string FullName,
    string? Phone,
    string? Email);

public sealed record GetEnrollmentCandidatesResult(
    IReadOnlyList<EnrollmentCandidateItem> Items,
    int Page,
    int PageSize,
    int TotalCount);

