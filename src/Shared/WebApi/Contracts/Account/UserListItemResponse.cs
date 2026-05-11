using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Account;

public sealed record UserListItemResponse(
    Guid UserId,
    string FullName,
    string Email,
    UserRole Role,
    string Status,
    DateTime? LastLoginAtUtc);

public sealed record UserListPagedResponse(
    IReadOnlyList<UserListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);
