namespace EduCrm.WebApi.Contracts.Account;

public sealed record UpdateProfileResponse(
    string Email,
    string FullName,
    string Initials);