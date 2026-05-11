namespace EduCrm.Modules.Account.Application.UseCases.UpdateUserByAdmin;

public sealed record UpdateUserByAdminInput(
    Guid CallerUserId,
    Guid CallerOrganizationId,
    Guid TargetUserId,
    string FullName);
