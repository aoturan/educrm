namespace EduCrm.WebApi.Contracts.Admin;

public sealed record SubscriptionRequestDetailResponse(
    SubscriptionRequestResponse Request,
    PaymentNotificationResponse? PaymentNotification);

public sealed record SubscriptionRequestResponse(
    Guid Id,
    Guid OrganizationId,
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
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record PaymentNotificationResponse(
    Guid Id,
    Guid SubscriptionRequestId,
    Guid OrganizationId,
    string SenderName,
    DateOnly PaymentDate,
    decimal Amount,
    string? Note,
    string? ReceiptObjectKey,
    string? ReceiptFileName,
    string? ReceiptContentType,
    long? ReceiptSizeBytes,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
