namespace EduCrm.WebApi.Contracts.Account;

public sealed record LoginRequest(
    string Email,
    string Password);
