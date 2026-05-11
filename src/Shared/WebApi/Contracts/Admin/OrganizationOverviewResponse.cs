namespace EduCrm.WebApi.Contracts.Admin;

public sealed record OrganizationOverviewResponse(
    Guid OrganizationId,
    string OrganizationName,
    string OwnerFullName,
    string OwnerEmail,
    string ContactPhone,
    DateTime CreatedAtUtc,
    OrganizationOverviewLimitsResponse Limits,
    OrganizationOverviewSubscriptionResponse? Subscription,
    OrganizationOverviewBillingDetailResponse? BillingDetail,
    IReadOnlyList<OrganizationOverviewSubscriptionRequestResponse> SubscriptionRequests);

public sealed record OrganizationOverviewLimitsResponse(
    string PlanCode,
    string Status,
    bool ExportEnabled,
    LimitUsageResponse Users,
    LimitUsageResponse ActivePersons,
    LimitUsageResponse ActivePrograms,
    LimitUsageResponse OpenFollowUps);

public sealed record LimitUsageResponse(int? Limit, int Current);

public sealed record OrganizationOverviewSubscriptionResponse(
    Guid Id,
    string PlanCode,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    string? DowngradedFromPlanCode,
    DateTime? DowngradedAtUtc,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record OrganizationOverviewBillingDetailResponse(
    Guid Id,
    string BillingType,
    string BillingName,
    string? TaxNumber,
    string? TaxOffice,
    string BillingEmail,
    string BillingAddress,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record OrganizationOverviewSubscriptionRequestResponse(
    Guid Id,
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
