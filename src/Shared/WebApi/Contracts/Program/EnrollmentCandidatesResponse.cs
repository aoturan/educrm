namespace EduCrm.WebApi.Contracts.Program;

public sealed record EnrollmentCandidateResponse(
    Guid PersonId,
    string FullName,
    string? Phone,
    string? Email);

public sealed record EnrollmentCandidatesPagedResponse(
    IReadOnlyList<EnrollmentCandidateResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);

