namespace EduCrm.Modules.Account.Application.UseCases.Login;

public sealed record LoginResult(string Token, string Email, string FullName, string Initials, string OrganizationName);
