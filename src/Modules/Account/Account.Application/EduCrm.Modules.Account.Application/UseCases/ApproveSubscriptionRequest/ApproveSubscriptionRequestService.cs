using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ApproveSubscriptionRequest;

public sealed class ApproveSubscriptionRequestService(
    ISubscriptionRequestRepository subscriptionRequestRepo,
    ISubscriptionRepository subscriptionRepo,
    ISubscriptionHistoryRepository subscriptionHistoryRepo,
    IUnitOfWork uow,
    IClock clock)
    : IApproveSubscriptionRequestService
{
    private static readonly TimeSpan SubscriptionDuration = TimeSpan.FromDays(365);

    public async Task<Result<ApproveSubscriptionRequestResult>> ApproveAsync(
        ApproveSubscriptionRequestInput input,
        CancellationToken ct)
    {
        var request = await subscriptionRequestRepo.GetTrackedByIdAsync(input.SubscriptionRequestId, ct);
        if (request is null)
            return Result<ApproveSubscriptionRequestResult>.Fail(
                AccountErrors.SubscriptionRequestNotFound(input.SubscriptionRequestId));

        if (request.Status is RequestStatus.Approved or RequestStatus.Rejected or RequestStatus.Cancelled)
            return Result<ApproveSubscriptionRequestResult>.Fail(
                AccountErrors.SubscriptionRequestAlreadyTerminal(request.Status.ToString()));

        var subscription = await subscriptionRepo.GetCurrentTrackedByOrganizationAsync(request.OrganizationId, ct);
        if (subscription is null)
            return Result<ApproveSubscriptionRequestResult>.Fail(
                AccountErrors.SubscriptionNotFound(request.OrganizationId));

        var now = clock.UtcNow.UtcDateTime;
        var endsAt = now + SubscriptionDuration;

        if (subscription.PlanCode != PlanCode.Free)
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

        request.Approve(now);
        subscription.ActivateFromRequest(
            request.RequestedPlanCode,
            now,
            endsAt,
            request.Id,
            now);

        await uow.SaveChangesAsync(ct);

        return Result<ApproveSubscriptionRequestResult>.Success(new ApproveSubscriptionRequestResult(
            request.Id,
            request.ApprovedAtUtc!.Value,
            subscription.Id,
            subscription.PlanCode.ToString(),
            subscription.StartsAtUtc,
            subscription.EndsAtUtc!.Value));
    }
}
