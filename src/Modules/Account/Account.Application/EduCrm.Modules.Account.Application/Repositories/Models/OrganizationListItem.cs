namespace EduCrm.Modules.Account.Application.Repositories.Models;

public sealed record OrganizationListItem(
    Guid Id,
    string Name,
    string ContactName,
    string ContactEmail,
    string ContactPhone,
    DateTime CreatedAtUtc);

public sealed record OrganizationPagedListQueryResult(
    IReadOnlyList<OrganizationListItem> Items,
    int TotalCount);
