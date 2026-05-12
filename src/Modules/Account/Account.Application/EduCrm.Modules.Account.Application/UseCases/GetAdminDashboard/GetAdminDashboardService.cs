using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Support.Contracts.Abstractions;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetAdminDashboard;

public sealed class GetAdminDashboardService(
    IOrganizationRepository organizationRepo,
    ISubscriptionRepository subscriptionRepo,
    ISubscriptionRequestRepository subscriptionRequestRepo,
    ISupportDashboardCountsProvider supportCounts,
    IClock clock) : IGetAdminDashboardService
{
    private const int PendingListTake = 20;
    private static readonly TimeSpan WeekWindow = TimeSpan.FromDays(7);

    public async Task<Result<GetAdminDashboardResult>> GetAsync(CancellationToken ct)
    {
        var now = clock.UtcNow.UtcDateTime;
        var weekAgo = now - WeekWindow;

        var pendingCount = await subscriptionRequestRepo.CountPendingAsync(ct);
        var newOrgsCount = await organizationRepo.CountCreatedSinceAsync(weekAgo, ct);
        var activePaidCount = await subscriptionRepo.CountActivePaidAsync(now, ct);
        var freeCount = await subscriptionRepo.CountFreeOrExpiredAsync(now, ct);
        var newContactMessages = await supportCounts.CountNewContactMessagesAsync(ct);
        var newSupportRequests = await supportCounts.CountNewRequestsAsync(ct);

        var pendingItems = await subscriptionRequestRepo.GetOldestPendingAsync(PendingListTake, ct);

        var items = pendingItems
            .Select(p => new AdminPendingPaymentRequestItem(
                p.SubscriptionRequestId,
                p.OrganizationId,
                p.OrganizationName,
                p.RequestedPlanCode,
                p.Status,
                p.Amount,
                p.PaymentReferenceCode,
                p.RequestedAtUtc,
                p.HasPaymentNotification))
            .ToList();

        return Result<GetAdminDashboardResult>.Success(new GetAdminDashboardResult(
            new AdminDashboardCountsResult(
                pendingCount,
                newOrgsCount,
                activePaidCount,
                freeCount,
                newContactMessages,
                newSupportRequests),
            items));
    }
}
