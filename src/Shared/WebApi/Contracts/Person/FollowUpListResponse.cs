using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Person;

public sealed record FollowUpListItemResponse(
    Guid Id,
    string PersonName,
    string? ProgramName,
    FollowUpType Type,
    FollowUpStatus Status,
    string Title,
    DateTime DueAtUtc,
    DateTime? SnoozedUntilUtc);

public sealed record FollowUpListPagedResponse(
    IReadOnlyList<FollowUpListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);

