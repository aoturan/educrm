using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.GetFollowUpById;

public interface IGetFollowUpByIdService
{
    Task<Result<GetFollowUpByIdResult>> GetAsync(Guid followUpId, CancellationToken ct);
}

