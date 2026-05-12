using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.ListSupportContactMessages;

public sealed class ListSupportContactMessagesService(
    ISupportContactMessageRepository supportContactMessageRepo)
    : IListSupportContactMessagesService
{
    public async Task<Result<ListSupportContactMessagesPagedResult>> ListAsync(
        ListSupportContactMessagesInput input,
        CancellationToken ct)
    {
        var queryResult = await supportContactMessageRepo.GetPagedListAsync(
            input.Page,
            input.PageSize,
            ct,
            input.PreFilter);

        return Result<ListSupportContactMessagesPagedResult>.Success(new ListSupportContactMessagesPagedResult(
            queryResult.Items.Select(x => new ListSupportContactMessagesItemResult(
                x.Id,
                x.FullName,
                x.Email,
                x.Subject,
                x.Message,
                x.Status.ToString(),
                x.CreatedAt,
                x.ReviewedAt)).ToList(),
            input.Page,
            input.PageSize,
            queryResult.TotalCount));
    }
}
