namespace EduCrm.Modules.Program.Application.UseCases.AssignPersonToApplication;

public sealed record AssignPersonInput(Guid ApplicationId);

public sealed record PersonCandidateData(
    Guid PersonId,
    string FullName,
    string? Email,
    string? Phone);

public sealed record AssignPersonResult(
    bool IsAmbiguous,
    IReadOnlyList<PersonCandidateData> Candidates);

