namespace EduCrm.Modules.Account.Contracts.Dtos;

public sealed record PlanUsageSnapshot(
    string PlanCode,
    string Status,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    string? DowngradedFromPlanCode,
    DateTime? DowngradedAtUtc,
    PlanLimits Limits,
    PendingSubscriptionRequestData? PendingRequest);

public sealed record PendingSubscriptionRequestData(
    string RequestedPlanCode,
    string Status,
    string PaymentMethod,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    bool HasPaymentNotification);