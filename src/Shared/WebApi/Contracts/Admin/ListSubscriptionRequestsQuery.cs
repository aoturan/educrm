namespace EduCrm.WebApi.Contracts.Admin;

public sealed record ListSubscriptionRequestsQuery(
    int Page = 1,
    int PageSize = 10,
    string? Q = null,
    string? Face = null);
