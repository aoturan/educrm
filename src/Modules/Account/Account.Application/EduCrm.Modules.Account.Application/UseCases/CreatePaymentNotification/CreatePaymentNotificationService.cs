using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.CreatePaymentNotification;

public sealed class CreatePaymentNotificationService(
    IUserRepository userRepo,
    ISubscriptionRequestRepository subscriptionRequestRepo,
    ISubscriptionPaymentNotificationRepository notificationRepo,
    IReceiptStorage receiptStorage,
    IUnitOfWork uow,
    IClock clock)
    : ICreatePaymentNotificationService
{
    public async Task<Result<CreatePaymentNotificationResult>> CreateAsync(
        CreatePaymentNotificationInput input,
        CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<CreatePaymentNotificationResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<CreatePaymentNotificationResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Role != UserRole.Admin)
            return Result<CreatePaymentNotificationResult>.Fail(AccountErrors.NotAdmin());

        var activeRequest = await subscriptionRequestRepo.GetActiveByOrganizationAsync(caller.OrganizationId, ct);
        if (activeRequest is null)
            return Result<CreatePaymentNotificationResult>.Fail(AccountErrors.NoActiveSubscriptionRequest());

        var alreadyHasNotification = await notificationRepo.ExistsBySubscriptionRequestIdAsync(activeRequest.Id, ct);
        if (alreadyHasNotification)
            return Result<CreatePaymentNotificationResult>.Fail(AccountErrors.PaymentNotificationAlreadyExists());

        var notificationId = Guid.NewGuid();
        var extension = Path.GetExtension(input.FileName).ToLowerInvariant();
        var objectKey = $"{caller.OrganizationId}/payment-notifications/{notificationId}{extension}";

        await receiptStorage.UploadAsync(objectKey, input.FileContent, input.ContentType, ct);

        var now = clock.UtcNow.UtcDateTime;
        var notification = new SubscriptionPaymentNotification(
            notificationId,
            activeRequest.Id,
            caller.OrganizationId,
            input.SenderName.Trim(),
            input.PaymentDate,
            input.Amount,
            string.IsNullOrWhiteSpace(input.Note) ? null : input.Note.Trim(),
            objectKey,
            input.FileName,
            input.ContentType,
            input.FileSizeBytes,
            now);

        notificationRepo.Add(notification);
        await uow.SaveChangesAsync(ct);

        return Result<CreatePaymentNotificationResult>.Success(new CreatePaymentNotificationResult(
            notification.Id,
            notification.SubscriptionRequestId,
            notification.SenderName,
            notification.PaymentDate,
            notification.Amount,
            notification.Note,
            notification.ReceiptFileName!,
            notification.ReceiptContentType!,
            notification.ReceiptSizeBytes!.Value,
            notification.CreatedAtUtc));
    }
}