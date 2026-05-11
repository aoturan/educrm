using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetSubscriptionRequestDetail;

public sealed class GetSubscriptionRequestDetailService(
    ISubscriptionRequestRepository subscriptionRequestRepo,
    ISubscriptionPaymentNotificationRepository paymentNotificationRepo)
    : IGetSubscriptionRequestDetailService
{
    public async Task<Result<GetSubscriptionRequestDetailResult>> GetAsync(GetSubscriptionRequestDetailInput input, CancellationToken ct)
    {
        var request = await subscriptionRequestRepo.GetByIdAsync(input.SubscriptionRequestId, ct);
        if (request is null)
            return Result<GetSubscriptionRequestDetailResult>.Fail(AccountErrors.SubscriptionRequestNotFound(input.SubscriptionRequestId));

        var notification = await paymentNotificationRepo.GetBySubscriptionRequestIdAsync(input.SubscriptionRequestId, ct);

        var requestResult = new SubscriptionRequestDetailResult(
            request.Id,
            request.OrganizationId,
            request.RequestedPlanCode.ToString(),
            request.Status.ToString(),
            request.PaymentMethod.ToString(),
            request.Amount,
            request.PaymentReferenceCode,
            request.RequestedAtUtc,
            request.ExpiresAtUtc,
            request.ApprovedAtUtc,
            request.RejectedAtUtc,
            request.CancelledAtUtc,
            request.IsInvoiced,
            request.CreatedAtUtc,
            request.UpdatedAtUtc);

        var notificationResult = notification is null
            ? null
            : new PaymentNotificationDetailResult(
                notification.Id,
                notification.SubscriptionRequestId,
                notification.OrganizationId,
                notification.SenderName,
                notification.PaymentDate,
                notification.Amount,
                notification.Note,
                notification.ReceiptObjectKey,
                notification.ReceiptFileName,
                notification.ReceiptContentType,
                notification.ReceiptSizeBytes,
                notification.CreatedAtUtc,
                notification.UpdatedAtUtc);

        return Result<GetSubscriptionRequestDetailResult>.Success(
            new GetSubscriptionRequestDetailResult(requestResult, notificationResult));
    }
}
