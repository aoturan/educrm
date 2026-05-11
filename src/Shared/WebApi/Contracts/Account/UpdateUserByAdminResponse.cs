using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Account;

public sealed record UpdateUserByAdminResponse(
    Guid UserId,
    string Email,
    string FullName,
    UserRole Role,
    string Status,
    DateTime? LastLoginAtUtc);
