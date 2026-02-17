namespace EduCrm.WebApi.Contracts.Account;

public sealed record UpdateProfileRequest(
    string FullName,
    string Email,
    string OrganizationName);
