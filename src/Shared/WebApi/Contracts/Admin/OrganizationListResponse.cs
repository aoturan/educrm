namespace EduCrm.WebApi.Contracts.Admin;

public sealed record OrganizationListItemResponse(
    Guid Id,
    string Name,
    string ContactName,
    string ContactEmail,
    string ContactPhone,
    DateTime CreatedAtUtc);

public sealed record OrganizationListPagedResponse(
    IReadOnlyList<OrganizationListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);
