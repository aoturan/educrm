namespace EduCrm.Modules.Support.Application.UseCases.ListSupportRequests;

public static class SupportRequestListPreFilter
{
    public const string New = "new";
    public const string Handled = "handled";
}

public sealed record ListSupportRequestsInput(
    int Page = 1,
    int PageSize = 10,
    string? PreFilter = null);
