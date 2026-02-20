using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.Repositories.Models;

public sealed record UserWithOrganization(
    Guid Id,
    Guid OrganizationId,
    UserStatus Status,
    string PasswordHash,
    string Email,
    string FullName,
    string OrganizationName);
