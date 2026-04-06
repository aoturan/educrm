using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.ChangeFollowUpStatus;

public sealed record ChangeFollowUpStatusInput(Guid FollowUpId, FollowUpStatus TargetStatus);

