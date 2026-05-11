using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.CreatePaymentNotification;

public interface ICreatePaymentNotificationService
{
    Task<Result<CreatePaymentNotificationResult>> CreateAsync(
        CreatePaymentNotificationInput input,
        CancellationToken ct);
}