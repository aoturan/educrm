namespace EduCrm.WebApi.Contracts.Person;

public sealed record UpdatePersonRequest(
    string FullName,
    string? Phone,
    string? Email,
    string? Notes);

