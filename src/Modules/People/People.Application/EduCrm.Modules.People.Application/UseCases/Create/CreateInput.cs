using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.Create;

public sealed record CreateInput(
    string FullName,
    SourceType Source,
    string? Phone = null,
    string? Email = null,
    string? Notes = null,
    Guid? ProgramId = null);
