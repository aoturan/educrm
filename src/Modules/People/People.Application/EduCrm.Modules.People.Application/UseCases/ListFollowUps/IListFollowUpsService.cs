using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ListFollowUps;

public interface IListFollowUpsService
{
    Task<Result<ListFollowUpsPagedResult>> ListAsync(ListFollowUpsInput input, CancellationToken ct);
}

