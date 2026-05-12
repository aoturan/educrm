using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.ListSupportRequests;

public interface IListSupportRequestsService
{
    Task<Result<ListSupportRequestsPagedResult>> ListAsync(
        ListSupportRequestsInput input,
        CancellationToken ct);
}
