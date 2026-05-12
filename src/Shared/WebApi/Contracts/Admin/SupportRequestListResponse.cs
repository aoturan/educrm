namespace EduCrm.WebApi.Contracts.Admin;

public sealed record SupportRequestListItemResponse(
    Guid Id,
    Guid OrganizationId,
    string OrganizationName,
    string? PageUrl,
    string Subject,
    string Message,
    string Status,
    DateTime CreatedAtUtc);

public sealed record SupportRequestListPagedResponse(
    IReadOnlyList<SupportRequestListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);
