namespace EduCrm.Modules.Support.Application.UseCases.ListSupportContactMessages;

public sealed record ListSupportContactMessagesItemResult(
    Guid Id,
    string FullName,
    string Email,
    string Subject,
    string Message,
    string Status,
    DateTime CreatedAt,
    DateTime? ReviewedAt);

public sealed record ListSupportContactMessagesPagedResult(
    IReadOnlyList<ListSupportContactMessagesItemResult> Items,
    int Page,
    int PageSize,
    int TotalCount);
