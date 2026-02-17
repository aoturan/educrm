namespace EduCrm.WebApi.Contracts.Account;

public sealed record RegisterRequest(
    string Name,
    string Email,
    string OrganizationName,
    string Password,
    string Phone);