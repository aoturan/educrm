using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetPlanUsage;

public interface IGetPlanUsageService
{
    Task<Result<GetPlanUsageResult>> GetAsync(GetPlanUsageInput input, CancellationToken ct);
}