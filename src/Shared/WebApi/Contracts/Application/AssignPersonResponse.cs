namespace EduCrm.WebApi.Contracts.Application;

public sealed record PersonCandidateResponse(
    Guid PersonId,
    string FullName,
    string? Email,
    string? Phone);

public sealed record AssignPersonResponse(
    bool IsAmbiguous,
    IReadOnlyList<PersonCandidateResponse> Candidates);

