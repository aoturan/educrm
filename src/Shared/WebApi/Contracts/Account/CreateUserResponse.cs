namespace EduCrm.WebApi.Contracts.Account;

public sealed record CreateUserResponse(Guid UserId, string Email, string FullName);