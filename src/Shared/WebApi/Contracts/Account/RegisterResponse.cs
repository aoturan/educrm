namespace EduCrm.WebApi.Contracts.Account;

public sealed record RegisterResponse(string Token, string Email, string FullName, string Initials, string OrganizationName);
