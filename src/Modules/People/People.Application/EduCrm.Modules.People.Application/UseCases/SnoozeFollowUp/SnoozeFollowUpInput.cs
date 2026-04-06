namespace EduCrm.Modules.People.Application.UseCases.SnoozeFollowUp;

public sealed record SnoozeFollowUpInput(Guid FollowUpId, DateTime SnoozeUntilUtc);

