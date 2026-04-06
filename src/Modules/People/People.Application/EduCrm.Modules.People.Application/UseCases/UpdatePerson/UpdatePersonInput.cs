namespace EduCrm.Modules.People.Application.UseCases.UpdatePerson;

public sealed record UpdatePersonInput(
    Guid PersonId,
    string FullName,
    string? Phone,
    string? Email,
    string? Notes);

