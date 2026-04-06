using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.ListFollowUps;

public sealed record FollowUpListItemResult(
    Guid Id,
    string PersonName,
    string? ProgramName,
    FollowUpType Type,
    FollowUpStatus Status,
    string Title,
    DateTime DueAtUtc,
    DateTime? SnoozedUntilUtc);

public sealed record ListFollowUpsPagedResult(
    IReadOnlyList<FollowUpListItemResult> Items,
    int TotalCount);

