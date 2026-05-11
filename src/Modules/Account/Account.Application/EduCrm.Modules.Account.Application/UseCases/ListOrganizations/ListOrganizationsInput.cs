namespace EduCrm.Modules.Account.Application.UseCases.ListOrganizations;

public static class OrganizationListFace
{
    public const string NewOrganizations = "newOrganizations";
    public const string ActivePaidOrganizations = "activePaidOrganizations";
    public const string FreeOrganizations = "freeOrganizations";
}

public sealed record ListOrganizationsInput(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? Face = null);
