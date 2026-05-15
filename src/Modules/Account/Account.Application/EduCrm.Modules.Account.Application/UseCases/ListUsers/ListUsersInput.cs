using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.ListUsers;

public sealed record ListUsersInput(
    int Page = 1,
    int PageSize = 10,
    IReadOnlyCollection<UserStatus>? Statuses = null);
