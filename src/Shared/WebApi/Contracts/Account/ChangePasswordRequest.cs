namespace EduCrm.WebApi.Contracts.Account;

public sealed record ChangePasswordRequest(
    string OldPassword,
    string NewPassword);

