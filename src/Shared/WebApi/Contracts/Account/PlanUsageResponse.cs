namespace EduCrm.WebApi.Contracts.Account;

public sealed record PlanUsageResponse(
    string PlanCode,
    string Status,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    string? DowngradedFromPlanCode,
    DateTime? DowngradedAtUtc,
    bool ExportEnabled,
    PendingRequestResponse? PendingRequest,
    LimitUsageResponse Users,
    LimitUsageResponse ActivePersons,
    LimitUsageResponse ActivePrograms,
    LimitUsageResponse OpenFollowUps);

public sealed record LimitUsageResponse(int? Limit, int Current);

public sealed record PendingRequestResponse(
    string RequestedPlanCode,
    string Status,
    string PaymentMethod,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    bool HasPaymentNotification);