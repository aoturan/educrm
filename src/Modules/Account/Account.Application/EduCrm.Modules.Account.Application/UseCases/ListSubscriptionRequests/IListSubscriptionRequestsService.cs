using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ListSubscriptionRequests;

public interface IListSubscriptionRequestsService
{
    Task<Result<ListSubscriptionRequestsPagedResult>> ListAsync(
        ListSubscriptionRequestsInput input,
        CancellationToken ct);
}
