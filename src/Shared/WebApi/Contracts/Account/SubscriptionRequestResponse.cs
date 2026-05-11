namespace EduCrm.WebApi.Contracts.Account;

public sealed record SubscriptionRequestResponse(
    Guid Id,
    string RequestedPlanCode,
    string Status,
    string PaymentMethod,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    DateTime ExpiresAtUtc);