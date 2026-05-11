using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.SubscriptionRequests;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetNotifications;

public sealed class GetNotificationsService(
    ISubscriptionRepository subscriptionRepo,
    ISubscriptionRequestRepository subscriptionRequestRepo,
    ISubscriptionHistoryRepository subscriptionHistoryRepo,
    IPlanPricingResolver planPricingResolver,
    IPaymentReferenceCodeGenerator referenceCodeGenerator,
    IUnitOfWork uow,
    IClock clock) : IGetNotificationsService
{
    private static readonly TimeSpan SubscriptionRequestLifetime = TimeSpan.FromDays(7);
    private const string PendingPaymentLink = "/settings/plans-and-limits";

    public async Task<Result<GetNotificationsResult>> GetAsync(Guid organizationId, CancellationToken ct)
    {
        var notifications = new List<NotificationItem>();
        var now = clock.UtcNow.UtcDateTime;
        var anyWrites = false;

        // 1) Cancel an expired pending SubscriptionRequest, if there is one.
        var activeRequest = await subscriptionRequestRepo.GetActiveByOrganizationAsync(organizationId, ct);
        var hadActiveRequestStillValid = false;

        if (activeRequest is not null)
        {
            if (activeRequest.ExpiresAtUtc < now)
            {
                activeRequest.Cancel(now);
                anyWrites = true;
            }
            else
            {
                hadActiveRequestStillValid = true;
            }
        }

        // 2) Detect & handle an expired Subscription: downgrade to Free + create a renewal request.
        var subscription = await subscriptionRepo.GetCurrentTrackedByOrganizationAsync(organizationId, ct);
        var subscriptionWasExpired = subscription is not null
                                  && subscription.PlanCode != PlanCode.Free
                                  && subscription.EndsAtUtc is { } endsAt
                                  && endsAt < now;

        if (subscriptionWasExpired)
        {
            var originalPlan = subscription!.PlanCode;
            var activatingRequestId = subscription.ActivatedBySubscriptionRequestId;

            decimal? snapshotAmount = null;
            PaymentMethod? snapshotPaymentMethod = null;
            string? snapshotPaymentReferenceCode = null;
            if (activatingRequestId is { } activatingId)
            {
                var activatingRequest = await subscriptionRequestRepo.GetByIdAsync(activatingId, ct);
                if (activatingRequest is not null)
                {
                    snapshotAmount = activatingRequest.Amount;
                    snapshotPaymentMethod = activatingRequest.PaymentMethod;
                    snapshotPaymentReferenceCode = activatingRequest.PaymentReferenceCode;
                }
            }

            subscriptionHistoryRepo.Add(new SubscriptionHistory(
                Guid.NewGuid(),
                organizationId,
                originalPlan,
                subscription.StartsAtUtc,
                subscription.EndsAtUtc!.Value,
                snapshotAmount,
                snapshotPaymentMethod,
                snapshotPaymentReferenceCode,
                activatingRequestId,
                now));

            subscription.DowngradeToFree(now);
            anyWrites = true;

            if (!hadActiveRequestStillValid)
            {
                var amount = planPricingResolver.GetPrice(originalPlan);
                var referenceCode = await referenceCodeGenerator.GenerateAsync(ct);

                subscriptionRequestRepo.Add(new SubscriptionRequest(
                    Guid.NewGuid(),
                    organizationId,
                    originalPlan,
                    RequestStatus.PendingPayment,
                    PaymentMethod.BankTransfer,
                    amount,
                    referenceCode,
                    now,
                    now.Add(SubscriptionRequestLifetime),
                    now));
            }
        }

        if (anyWrites)
            await uow.SaveChangesAsync(ct);

        // 3) Notifications based on the final state.
        if (subscriptionWasExpired)
        {
            notifications.Add(new NotificationItem(
                Message: "Plan süreniz doldu. Devam etmek için ödeme yapmanız gerekmektedir.",
                Link: PendingPaymentLink));
        }
        else if (hadActiveRequestStillValid)
        {
            notifications.Add(new NotificationItem(
                Message: "Bekleyen bir ödeme isteğiniz mevcuttur.",
                Link: PendingPaymentLink));
        }

        return Result<GetNotificationsResult>.Success(new GetNotificationsResult(notifications));
    }
}
