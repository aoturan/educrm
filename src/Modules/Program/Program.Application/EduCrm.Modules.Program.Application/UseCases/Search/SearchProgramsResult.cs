using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.Search;

public sealed record SearchProgramsResult(
    Guid Id,
    string Name,
    string PublicShortDescription,
    ProgramStatus Status,
    DateOnly StartDate,
    DateOnly EndDate,
    int EnrollmentCount,
    bool IsArchived);

