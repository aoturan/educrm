using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.DownloadPaymentNotificationReceipt;

public sealed class DownloadPaymentNotificationReceiptService(
    ISubscriptionRequestRepository subscriptionRequestRepo,
    ISubscriptionPaymentNotificationRepository notificationRepo,
    IReceiptStorage receiptStorage)
    : IDownloadPaymentNotificationReceiptService
{
    public async Task<Result<DownloadPaymentNotificationReceiptResult>> DownloadAsync(
        DownloadPaymentNotificationReceiptInput input,
        CancellationToken ct)
    {
        var request = await subscriptionRequestRepo.GetByIdAsync(input.SubscriptionRequestId, ct);
        if (request is null)
            return Result<DownloadPaymentNotificationReceiptResult>.Fail(
                AccountErrors.SubscriptionRequestNotFound(input.SubscriptionRequestId));

        var notification = await notificationRepo.GetBySubscriptionRequestIdAsync(input.SubscriptionRequestId, ct);
        if (notification is null
            || string.IsNullOrWhiteSpace(notification.ReceiptObjectKey)
            || string.IsNullOrWhiteSpace(notification.ReceiptFileName)
            || string.IsNullOrWhiteSpace(notification.ReceiptContentType))
        {
            return Result<DownloadPaymentNotificationReceiptResult>.Fail(
                AccountErrors.PaymentNotificationReceiptNotFound(input.SubscriptionRequestId));
        }

        var stream = await receiptStorage.DownloadAsync(notification.ReceiptObjectKey, ct);

        return Result<DownloadPaymentNotificationReceiptResult>.Success(
            new DownloadPaymentNotificationReceiptResult(
                stream,
                notification.ReceiptFileName,
                notification.ReceiptContentType));
    }
}
