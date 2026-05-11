namespace EduCrm.WebApi.Contracts.Account;

public sealed record UpdateUserByAdminRequest(
    Guid UserId,
    string FullName);
