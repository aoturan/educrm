namespace EduCrm.WebApi.Contracts.Program;

public sealed record ListProgramsQuery(
    int Page = 1,
    int PageSize = 10,
    string? Q = null,
    string? PreFilter = null,
    bool ShowArchived = false,
    Guid? PersonId = null,
    string? Face = null);

