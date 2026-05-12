using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.ListSupportContactMessages;

public interface IListSupportContactMessagesService
{
    Task<Result<ListSupportContactMessagesPagedResult>> ListAsync(
        ListSupportContactMessagesInput input,
        CancellationToken ct);
}
