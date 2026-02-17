namespace EduCrm.Modules.Account.Application.UseCases.Register;

public sealed record RegisterResult(string Token, string Email, string FullName, string Initials, string OrganizationName);
