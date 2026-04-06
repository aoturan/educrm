using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Person;

public sealed record RescheduleDueDateResponse(
    Guid FollowUpId,
    DateTime DueAtUtc,
    FollowUpStatus Status,
    DateTime? SnoozedUntilUtc);

