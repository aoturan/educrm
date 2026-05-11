using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.DownloadPaymentNotificationReceipt;

public interface IDownloadPaymentNotificationReceiptService
{
    Task<Result<DownloadPaymentNotificationReceiptResult>> DownloadAsync(
        DownloadPaymentNotificationReceiptInput input,
        CancellationToken ct);
}
