namespace EduCrm.Modules.Account.Application.UseCases.CreatePaymentNotification;

public sealed record CreatePaymentNotificationResult(
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