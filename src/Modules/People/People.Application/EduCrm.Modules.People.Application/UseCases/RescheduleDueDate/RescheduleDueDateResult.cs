using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.RescheduleDueDate;

public sealed record RescheduleDueDateResult(
    Guid FollowUpId,
    DateTime DueAtUtc,
    FollowUpStatus Status,
    DateTime? SnoozedUntilUtc);

