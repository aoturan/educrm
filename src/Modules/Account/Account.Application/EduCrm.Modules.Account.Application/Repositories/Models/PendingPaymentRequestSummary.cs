namespace EduCrm.Modules.Account.Application.Repositories.Models;

public sealed record PendingPaymentRequestSummary(
    Guid SubscriptionRequestId,
    Guid OrganizationId,
    string OrganizationName,
    string RequestedPlanCode,
    string Status,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    bool HasPaymentNotification);
