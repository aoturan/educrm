using EduCrm.Modules.Support.Domain.Enums;

namespace EduCrm.Modules.Support.Application.Repositories.Models;

public sealed record SupportRequestListItem(
    Guid Id,
    Guid OrganizationId,
    string OrganizationName,
    string? PageUrl,
    string Subject,
    string Message,
    SupportRequestStatus Status,
    DateTime CreatedAtUtc);

public sealed record SupportRequestPagedListQueryResult(
    IReadOnlyList<SupportRequestListItem> Items,
    int TotalCount);
