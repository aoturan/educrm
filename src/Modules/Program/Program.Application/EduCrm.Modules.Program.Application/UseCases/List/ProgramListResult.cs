using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.List;

public sealed record ProgramListResult(
    Guid Id,
    string Name,
    string PublicShortDescription,
    ProgramStatus Status,
    DateOnly StartDate,
    DateOnly EndDate,
    int EnrollmentCount,
    bool IsArchived);

public sealed record ProgramListPagedResult(
    IReadOnlyList<ProgramListResult> Items,
    int Page,
    int PageSize,
    int TotalCount);
