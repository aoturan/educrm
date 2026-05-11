namespace EduCrm.WebApi.Contracts.Account;

public sealed record ChangeUserPasswordByAdminRequest(
    Guid UserId,
    string Password);
