using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.ListSupportRequests;

public sealed class ListSupportRequestsService(
    ISupportRequestRepository supportRequestRepo)
    : IListSupportRequestsService
{
    public async Task<Result<ListSupportRequestsPagedResult>> ListAsync(
        ListSupportRequestsInput input,
        CancellationToken ct)
    {
        var queryResult = await supportRequestRepo.GetPagedListAsync(
            input.Page,
            input.PageSize,
            ct,
            input.PreFilter);

        return Result<ListSupportRequestsPagedResult>.Success(new ListSupportRequestsPagedResult(
            queryResult.Items.Select(x => new ListSupportRequestsItemResult(
                x.Id,
                x.OrganizationId,
                x.OrganizationName,
                x.PageUrl,
                x.Subject,
                x.Message,
                x.Status.ToString(),
                x.CreatedAtUtc)).ToList(),
            input.Page,
            input.PageSize,
            queryResult.TotalCount));
    }
}
