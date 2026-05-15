namespace EduCrm.Modules.Account.Application.UseCases.CreatePaymentNotification;

public sealed record CreatePaymentNotificationInput(
    string SenderName,
    DateOnly PaymentDate,
    decimal Amount,
    string? Note,
    Stream FileContent,
    string FileName,
    string ContentType,
    long FileSizeBytes);
