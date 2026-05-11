using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.Repositories.Models;

public sealed record SubscriptionRequestListItem(
    Guid Id,
    Guid OrganizationId,
    string OrganizationName,
    PlanCode RequestedPlanCode,
    RequestStatus Status,
    PaymentMethod PaymentMethod,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    DateTime ExpiresAtUtc,
    DateTime? ApprovedAtUtc,
    DateTime? RejectedAtUtc,
    DateTime? CancelledAtUtc,
    bool IsInvoiced,
    bool HasPaymentNotification);

public sealed record SubscriptionRequestPagedListQueryResult(
    IReadOnlyList<SubscriptionRequestListItem> Items,
    int TotalCount);
