namespace EduCrm.Modules.Account.Application.UseCases.TransferAdminRole;

public sealed record TransferAdminRoleInput(
    Guid CallerUserId,
    Guid CallerOrganizationId,
    Guid TargetUserId);
