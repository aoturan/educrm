namespace EduCrm.Modules.Account.Application.UseCases.ResetPassword;

public sealed record ResetPasswordInput(
    string Email,
    string Token,
    string NewPassword);
