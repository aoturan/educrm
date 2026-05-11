namespace EduCrm.Modules.Account.Application.UseCases.CreateSubscriptionRequest;

public sealed record CreateSubscriptionRequestResult(
    Guid Id,
    string RequestedPlanCode,
    string Status,
    string PaymentMethod,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    DateTime ExpiresAtUtc);