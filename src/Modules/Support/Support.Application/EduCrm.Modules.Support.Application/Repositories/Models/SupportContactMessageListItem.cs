using EduCrm.Modules.Support.Domain.Enums;

namespace EduCrm.Modules.Support.Application.Repositories.Models;

public sealed record SupportContactMessageListItem(
    Guid Id,
    string FullName,
    string Email,
    string Subject,
    string Message,
    SupportContactMessageStatus Status,
    DateTime CreatedAt,
    DateTime? ReviewedAt);

public sealed record SupportContactMessagePagedListQueryResult(
    IReadOnlyList<SupportContactMessageListItem> Items,
    int TotalCount);
