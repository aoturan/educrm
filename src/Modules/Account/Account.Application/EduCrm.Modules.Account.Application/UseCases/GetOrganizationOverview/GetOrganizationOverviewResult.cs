namespace EduCrm.Modules.Account.Application.UseCases.GetOrganizationOverview;

public sealed record GetOrganizationOverviewResult(
    Guid OrganizationId,
    string OrganizationName,
    string OwnerFullName,
    string OwnerEmail,
    string ContactPhone,
    DateTime CreatedAtUtc,
    OrganizationOverviewLimitsResult Limits,
    OrganizationOverviewSubscriptionResult? Subscription,
    OrganizationOverviewBillingDetailResult? BillingDetail,
    IReadOnlyList<OrganizationOverviewSubscriptionRequestResult> SubscriptionRequests);

public sealed record OrganizationOverviewLimitsResult(
    string PlanCode,
    string Status,
    bool ExportEnabled,
    LimitUsageResult Users,
    LimitUsageResult ActivePersons,
    LimitUsageResult ActivePrograms,
    LimitUsageResult OpenFollowUps);

public sealed record LimitUsageResult(int? Limit, int Current);

public sealed record OrganizationOverviewSubscriptionResult(
    Guid Id,
    string PlanCode,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    string? DowngradedFromPlanCode,
    DateTime? DowngradedAtUtc,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record OrganizationOverviewBillingDetailResult(
    Guid Id,
    string BillingType,
    string BillingName,
    string? TaxNumber,
    string? TaxOffice,
    string BillingEmail,
    string BillingAddress,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record OrganizationOverviewSubscriptionRequestResult(
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
