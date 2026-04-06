namespace EduCrm.Modules.People.Application.UseCases.SnoozeFollowUp;

public sealed record SnoozeFollowUpResult(Guid FollowUpId, DateTime SnoozedUntilUtc);

