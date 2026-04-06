using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.ChangeFollowUpStatus;

public sealed record ChangeFollowUpStatusResult(
    Guid FollowUpId,
    FollowUpStatus Status,
    DateTime? CompletedAtUtc,
    DateTime? CancelledAtUtc);

