using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetMe;

public interface IGetMeService
{
    Task<Result<GetMeResult>> GetMeAsync(Guid userId, CancellationToken ct);
}

