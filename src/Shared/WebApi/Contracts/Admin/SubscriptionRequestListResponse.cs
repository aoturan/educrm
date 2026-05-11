namespace EduCrm.WebApi.Contracts.Admin;

public sealed record SubscriptionRequestListItemResponse(
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

public sealed record SubscriptionRequestListPagedResponse(
    IReadOnlyList<SubscriptionRequestListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);
