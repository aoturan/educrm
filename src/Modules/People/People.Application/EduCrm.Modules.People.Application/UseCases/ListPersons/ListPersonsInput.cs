namespace EduCrm.Modules.People.Application.UseCases.ListPersons;

public static class PersonPreFilter
{
    public const string Enrolled = "enrolled";
    public const string NotEnrolled = "not-enrolled";
}

public sealed record ListPersonsInput(int Page, int PageSize, string? SearchTerm = null, string? PreFilter = null, bool ShowArchived = false);



