namespace EduCrm.Modules.People.Application.UseCases.RescheduleDueDate;

public sealed record RescheduleDueDateInput(Guid FollowUpId, DateTime NewDueAtUtc);

