using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.ChangeUserStatus;

public sealed record ChangeUserStatusInput(
    Guid CallerUserId,
    Guid CallerOrganizationId,
    Guid TargetUserId,
    UserStatusOperation Operation);