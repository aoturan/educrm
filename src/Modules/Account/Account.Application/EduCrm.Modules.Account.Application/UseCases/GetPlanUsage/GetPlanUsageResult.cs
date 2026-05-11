namespace EduCrm.Modules.Account.Application.UseCases.GetPlanUsage;

public sealed record GetPlanUsageResult(
    string PlanCode,
    string Status,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    string? DowngradedFromPlanCode,
    DateTime? DowngradedAtUtc,
    bool ExportEnabled,
    PendingRequestResult? PendingRequest,
    LimitUsageResult Users,
    LimitUsageResult ActivePersons,
    LimitUsageResult ActivePrograms,
    LimitUsageResult OpenFollowUps);

public sealed record LimitUsageResult(int? Limit, int Current);

public sealed record PendingRequestResult(
    string RequestedPlanCode,
    string Status,
    string PaymentMethod,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    bool HasPaymentNotification);