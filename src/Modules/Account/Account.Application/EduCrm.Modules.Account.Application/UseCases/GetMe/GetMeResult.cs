namespace EduCrm.Modules.Account.Application.UseCases.GetMe;

public sealed record GetMeResult(
    string Email,
    string FullName,
    string Initials,
    string OrganizationName);

