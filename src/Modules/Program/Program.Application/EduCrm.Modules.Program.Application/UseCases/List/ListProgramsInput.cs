namespace EduCrm.Modules.Program.Application.UseCases.List;

public static class ProgramListPreFilter
{
    public const string Active = "active";
    public const string Completed = "completed";
    public const string NotStarted = "not-started";
    public const string Ongoing = "ongoing";
}

public static class ProgramListFace
{
    public const string Approaching = "approaching";
}

public sealed record ListProgramsInput(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? PreFilter = null,
    bool ShowArchived = false,
    Guid? PersonId = null,
    string? Face = null);
