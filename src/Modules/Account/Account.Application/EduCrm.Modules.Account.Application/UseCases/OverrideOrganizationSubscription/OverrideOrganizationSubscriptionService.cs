using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.OverrideOrganizationSubscription;

public sealed class OverrideOrganizationSubscriptionService(
    IOrganizationRepository organizationRepo,
    ISubscriptionRepository subscriptionRepo,
    ISubscriptionRequestRepository subscriptionRequestRepo,
    ISubscriptionHistoryRepository subscriptionHistoryRepo,
    IUnitOfWork uow,
    IClock clock)
    : IOverrideOrganizationSubscriptionService
{
    public async Task<Result<OverrideOrganizationSubscriptionResult>> OverrideAsync(
        OverrideOrganizationSubscriptionInput input,
        CancellationToken ct)
    {
        if (input.EndsAtUtc <= input.StartsAtUtc)
            return Result<OverrideOrganizationSubscriptionResult>.Fail(
                AccountErrors.InvalidSubscriptionPeriod());

        var organization = await organizationRepo.GetByIdAsync(input.OrganizationId, ct);
        if (organization is null)
            return Result<OverrideOrganizationSubscriptionResult>.Fail(
                AccountErrors.OrganizationNotFound(input.OrganizationId));

        var subscription = await subscriptionRepo.GetCurrentTrackedByOrganizationAsync(input.OrganizationId, ct);
        if (subscription is null)
            return Result<OverrideOrganizationSubscriptionResult>.Fail(
                AccountErrors.SubscriptionNotFound(input.OrganizationId));

        var now = clock.UtcNow.UtcDateTime;
        var planChanged = subscription.PlanCode != input.PlanCode;

        if (planChanged && subscription.PlanCode != PlanCode.Free)
        {
            decimal? snapshotAmount = null;
            PaymentMethod? snapshotPaymentMethod = null;
            string? snapshotPaymentReferenceCode = null;

            if (subscription.ActivatedBySubscriptionRequestId is { } previousRequestId)
            {
                var previousRequest = await subscriptionRequestRepo.GetByIdAsync(previousRequestId, ct);
                if (previousRequest is not null)
                {
                    snapshotAmount = previousRequest.Amount;
                    snapshotPaymentMethod = previousRequest.PaymentMethod;
                    snapshotPaymentReferenceCode = previousRequest.PaymentReferenceCode;
                }
            }

            subscriptionHistoryRepo.Add(new SubscriptionHistory(
                Guid.NewGuid(),
                subscription.OrganizationId,
                subscription.PlanCode,
                subscription.StartsAtUtc,
                now,
                snapshotAmount,
                snapshotPaymentMethod,
                snapshotPaymentReferenceCode,
                subscription.ActivatedBySubscriptionRequestId,
                now));
        }

        subscription.OverrideByAdmin(input.PlanCode, input.StartsAtUtc, input.EndsAtUtc, now);

        var pendingRequest = await subscriptionRequestRepo.GetActiveByOrganizationAsync(input.OrganizationId, ct);
        pendingRequest?.Reject(now);

        await uow.SaveChangesAsync(ct);

        return Result<OverrideOrganizationSubscriptionResult>.Success(
            new OverrideOrganizationSubscriptionResult(
                subscription.Id,
                subscription.OrganizationId,
                subscription.PlanCode.ToString(),
                subscription.StartsAtUtc,
                subscription.EndsAtUtc!.Value,
                subscription.UpdatedAtUtc));
    }
}
