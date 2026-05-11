using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ListSubscriptionRequests;

public sealed class ListSubscriptionRequestsService(
    ISubscriptionRequestRepository subscriptionRequestRepo)
    : IListSubscriptionRequestsService
{
    public async Task<Result<ListSubscriptionRequestsPagedResult>> ListAsync(
        ListSubscriptionRequestsInput input,
        CancellationToken ct)
    {
        var queryResult = await subscriptionRequestRepo.GetPagedListAsync(
            input.Page,
            input.PageSize,
            ct,
            input.SearchTerm,
            input.Face);

        return Result<ListSubscriptionRequestsPagedResult>.Success(new ListSubscriptionRequestsPagedResult(
            queryResult.Items.Select(x => new ListSubscriptionRequestsItemResult(
                x.Id,
                x.OrganizationId,
                x.OrganizationName,
                x.RequestedPlanCode.ToString(),
                x.Status.ToString(),
                x.PaymentMethod.ToString(),
                x.Amount,
                x.PaymentReferenceCode,
                x.RequestedAtUtc,
                x.ExpiresAtUtc,
                x.ApprovedAtUtc,
                x.RejectedAtUtc,
                x.CancelledAtUtc,
                x.IsInvoiced,
                x.HasPaymentNotification)).ToList(),
            input.Page,
            input.PageSize,
            queryResult.TotalCount));
    }
}
