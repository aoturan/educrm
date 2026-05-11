namespace EduCrm.WebApi.Contracts.Account;

public sealed record PaymentNotificationResponse(
    Guid Id,
    Guid SubscriptionRequestId,
    string SenderName,
    DateOnly PaymentDate,
    decimal Amount,
    string? Note,
    string ReceiptFileName,
    string ReceiptContentType,
    long ReceiptSizeBytes,
    DateTime CreatedAtUtc);