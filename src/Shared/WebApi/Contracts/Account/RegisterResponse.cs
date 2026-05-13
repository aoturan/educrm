using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Account;

public sealed record RegisterResponse(
    string Email,
    UserStatus Status,
    string? Token,
    string? FullName,
    string? Initials,
    string? OrganizationName,
    string? Role);
