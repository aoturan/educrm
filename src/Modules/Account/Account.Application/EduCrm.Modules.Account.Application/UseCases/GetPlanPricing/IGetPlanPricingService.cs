using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetPlanPricing;

public interface IGetPlanPricingService
{
    Task<Result<GetPlanPricingResult>> GetAsync(CancellationToken ct);
}