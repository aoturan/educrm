namespace EduCrm.Modules.Account.Application.UseCases.MarkSubscriptionRequestInvoiced;

public sealed record MarkSubscriptionRequestInvoicedResult(
    Guid SubscriptionRequestId,
    bool IsInvoiced,
    DateTime UpdatedAtUtc);
