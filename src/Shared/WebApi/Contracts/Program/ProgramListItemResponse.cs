using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Program;

public sealed record ProgramListItemResponse(
    Guid Id,
    string Name,
    string PublicShortDescription,
    ProgramStatus Status,
    DateOnly StartDate,
    DateOnly EndDate,
    int EnrollmentCount,
    bool IsArchived);

public sealed record ProgramListPagedResponse(
    IReadOnlyList<ProgramListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);
