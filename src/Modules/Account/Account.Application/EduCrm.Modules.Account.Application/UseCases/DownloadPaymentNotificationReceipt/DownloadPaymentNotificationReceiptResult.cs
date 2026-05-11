namespace EduCrm.Modules.Account.Application.UseCases.DownloadPaymentNotificationReceipt;

public sealed record DownloadPaymentNotificationReceiptResult(
    Stream Content,
    string FileName,
    string ContentType);
