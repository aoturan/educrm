using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.Repositories.Models;

public sealed record UserListItem(
    Guid UserId,
    string FullName,
    string Email,
    UserRole Role,
    UserStatus Status,
    DateTime? LastLoginAtUtc);

public sealed record UserPagedListQueryResult(
    IReadOnlyList<UserListItem> Items,
    int TotalCount);
