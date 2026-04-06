using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Person;

public sealed record ChangeFollowUpStatusResponse(
    Guid FollowUpId,
    FollowUpStatus Status,
    DateTime? CompletedAtUtc,
    DateTime? CancelledAtUtc);

