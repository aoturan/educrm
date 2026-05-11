namespace EduCrm.WebApi.Contracts.Admin;

public sealed record MarkSubscriptionRequestInvoicedResponse(
    Guid SubscriptionRequestId,
    bool IsInvoiced,
    DateTime UpdatedAtUtc);
