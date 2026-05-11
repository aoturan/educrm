namespace EduCrm.Modules.Account.Application.UseCases.ListOrganizations;

public sealed record ListOrganizationsItemResult(
    Guid Id,
    string Name,
    string ContactName,
    string ContactEmail,
    string ContactPhone,
    DateTime CreatedAtUtc);

public sealed record ListOrganizationsPagedResult(
    IReadOnlyList<ListOrganizationsItemResult> Items,
    int Page,
    int PageSize,
    int TotalCount);
