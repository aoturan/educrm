using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.ChangeUserStatus;

public sealed record ChangeUserStatusInput(
    Guid TargetUserId,
    UserStatus TargetStatus);
