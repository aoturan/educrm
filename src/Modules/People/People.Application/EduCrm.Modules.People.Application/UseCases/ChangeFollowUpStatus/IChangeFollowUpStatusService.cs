using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ChangeFollowUpStatus;

public interface IChangeFollowUpStatusService
{
    Task<Result<ChangeFollowUpStatusResult>> ChangeAsync(ChangeFollowUpStatusInput input, CancellationToken ct);
}

