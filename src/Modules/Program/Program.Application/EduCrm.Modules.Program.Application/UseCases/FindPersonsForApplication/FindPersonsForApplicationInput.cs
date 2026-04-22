namespace EduCrm.Modules.Program.Application.UseCases.FindPersonsForApplication;

public sealed record FindPersonsForApplicationInput(Guid ApplicationId);

public sealed record PersonMatchData(
    Guid PersonId,
    string FullName,
    string? Email,
    string? Phone);

public sealed record FindPersonsForApplicationResult(
    IReadOnlyList<PersonMatchData> Persons);

