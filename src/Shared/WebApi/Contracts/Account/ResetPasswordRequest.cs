namespace EduCrm.WebApi.Contracts.Account;

public sealed record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword);
