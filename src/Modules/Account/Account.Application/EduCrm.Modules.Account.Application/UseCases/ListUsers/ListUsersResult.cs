using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.ListUsers;

public sealed record ListUsersItemResult(
    Guid UserId,
    string FullName,
    string Email,
    UserRole Role,
    UserStatus Status,
    DateTime? LastLoginAtUtc);

public sealed record ListUsersPagedResult(
    IReadOnlyList<ListUsersItemResult> Items,
    int Page,
    int PageSize,
    int TotalCount);
