namespace EduCrm.Modules.Account.Application.UseCases.ChangePassword;

public sealed record ChangePasswordInput(
    Guid UserId,
    string OldPassword,
    string NewPasswordHash);

