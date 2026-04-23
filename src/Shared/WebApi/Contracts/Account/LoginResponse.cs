using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Account;

public sealed record LoginResponse(
    string Token,
    string Email,
    string FullName,
    string Initials,
    string OrganizationName,
    UserRole Role);