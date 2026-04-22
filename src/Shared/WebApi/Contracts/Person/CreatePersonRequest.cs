namespace EduCrm.WebApi.Contracts.Person;

public sealed record CreatePersonRequest(
    string FullName,
    string? Phone,
    string? Email,
    Guid? ProgramId,
    string? Notes = null);
