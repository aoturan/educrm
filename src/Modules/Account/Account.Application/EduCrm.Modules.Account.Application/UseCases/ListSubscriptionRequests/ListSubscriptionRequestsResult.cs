namespace EduCrm.Modules.Account.Application.UseCases.ListSubscriptionRequests;

public sealed record ListSubscriptionRequestsItemResult(
    Guid Id,
    Guid OrganizationId,
    string OrganizationName,
    string RequestedPlanCode,
    string Status,
    string PaymentMethod,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    DateTime ExpiresAtUtc,
    DateTime? ApprovedAtUtc,
    DateTime? RejectedAtUtc,
    DateTime? CancelledAtUtc,
    bool IsInvoiced,
    bool HasPaymentNotification);

public sealed record ListSubscriptionRequestsPagedResult(
    IReadOnlyList<ListSubscriptionRequestsItemResult> Items,
    int Page,
    int PageSize,
    int TotalCount);
