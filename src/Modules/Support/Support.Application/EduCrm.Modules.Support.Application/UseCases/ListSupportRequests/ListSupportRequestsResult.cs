namespace EduCrm.Modules.Support.Application.UseCases.ListSupportRequests;

public sealed record ListSupportRequestsItemResult(
    Guid Id,
    Guid OrganizationId,
    string OrganizationName,
    string? PageUrl,
    string Subject,
    string Message,
    string Status,
    DateTime CreatedAtUtc);

public sealed record ListSupportRequestsPagedResult(
    IReadOnlyList<ListSupportRequestsItemResult> Items,
    int Page,
    int PageSize,
    int TotalCount);
