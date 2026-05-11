namespace EduCrm.Modules.Account.Application.UseCases.ListSubscriptionRequests;

public static class SubscriptionRequestListFace
{
    public const string Pending = "pending";
    public const string Approved = "approved";
    public const string Rejected = "rejected";
    public const string Cancelled = "cancelled";
}

public sealed record ListSubscriptionRequestsInput(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? Face = null);
