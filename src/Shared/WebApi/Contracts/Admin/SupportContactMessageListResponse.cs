namespace EduCrm.WebApi.Contracts.Admin;

public sealed record SupportContactMessageListItemResponse(
    Guid Id,
    string FullName,
    string Email,
    string Subject,
    string Message,
    string Status,
    DateTime CreatedAt,
    DateTime? ReviewedAt);

public sealed record SupportContactMessageListPagedResponse(
    IReadOnlyList<SupportContactMessageListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);
