namespace EduCrm.Modules.Account.Application.UseCases.UpdateProfile;

public sealed record UpdateProfileInput(
    Guid UserId,
    Guid OrganizationId,
    string FullName,
    string Email,
    string OrganizationName);

