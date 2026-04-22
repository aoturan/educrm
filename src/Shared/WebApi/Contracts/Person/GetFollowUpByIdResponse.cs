using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Person;

public sealed record FollowUpPersonResponse(Guid Id, string Name, string? Email, string? Phone);
public sealed record FollowUpProgramResponse(Guid Id, string Name, DateOnly StartDate, DateOnly EndDate);

public sealed record GetFollowUpByIdResponse(
    Guid Id,
    Guid OrganizationId,
    FollowUpType Type,
    FollowUpStatus Status,
    string Title,
    string? Note,
    DateTime DueAtUtc,
    DateTime? SnoozedUntilUtc,
    DateTime? CompletedAtUtc,
    DateTime? CancelledAtUtc,
    FollowUpPersonResponse Person,
    FollowUpProgramResponse? Program);
