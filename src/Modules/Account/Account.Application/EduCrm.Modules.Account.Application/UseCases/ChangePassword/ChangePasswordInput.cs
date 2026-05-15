namespace EduCrm.Modules.Account.Application.UseCases.ChangePassword;

public sealed record ChangePasswordInput(
    string OldPassword,
    string NewPasswordHash);
