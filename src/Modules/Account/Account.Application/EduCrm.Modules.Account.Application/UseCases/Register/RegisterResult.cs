using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.Register;

public sealed record RegisterResult(
    string Email,
    UserStatus Status,
    string? Token,
    string? FullName,
    string? Initials,
    string? OrganizationName,
    string? Role);
