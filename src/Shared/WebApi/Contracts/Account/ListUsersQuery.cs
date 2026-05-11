namespace EduCrm.WebApi.Contracts.Account;

public sealed record ListUsersQuery(
    int Page = 1,
    int PageSize = 10,
    string? Status = null);
