namespace EduCrm.Modules.Account.Application.UseCases.ChangeUserPasswordByAdmin;

public sealed record ChangeUserPasswordByAdminInput(
    Guid TargetUserId,
    string NewPasswordHash);
