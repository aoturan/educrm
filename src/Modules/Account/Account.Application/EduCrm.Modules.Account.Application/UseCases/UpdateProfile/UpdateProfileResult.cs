namespace EduCrm.Modules.Account.Application.UseCases.UpdateProfile;

public sealed record UpdateProfileResult(
    string Email,
    string FullName,
    string Initials,
    string OrganizationName);

