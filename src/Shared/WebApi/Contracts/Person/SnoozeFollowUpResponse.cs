namespace EduCrm.WebApi.Contracts.Person;

public sealed record SnoozeFollowUpResponse(Guid FollowUpId, DateTime SnoozedUntilUtc);

