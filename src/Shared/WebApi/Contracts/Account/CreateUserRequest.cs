namespace EduCrm.WebApi.Contracts.Account;

public sealed record CreateUserRequest(
    string Name,
    string Email,
    string Password);