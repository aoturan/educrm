using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.UpdateUserByAdmin;

public sealed record UpdateUserByAdminResult(
    Guid UserId,
    string Email,
    string FullName,
    UserRole Role,
    UserStatus Status,
    DateTime? LastLoginAtUtc);
