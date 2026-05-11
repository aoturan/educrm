namespace EduCrm.Modules.Account.Application.UseCases.ChangeUserPasswordByAdmin;

public sealed record ChangeUserPasswordByAdminInput(
    Guid CallerUserId,
    Guid CallerOrganizationId,
    Guid TargetUserId,
    string NewPasswordHash);
