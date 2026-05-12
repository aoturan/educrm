namespace EduCrm.WebApi.Contracts.Admin;

public sealed record ListSupportContactMessagesQuery(
    int Page = 1,
    int PageSize = 10,
    string? PreFilter = null);
