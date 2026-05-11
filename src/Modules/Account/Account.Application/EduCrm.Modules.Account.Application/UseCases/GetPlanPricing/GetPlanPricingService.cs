using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetPlanPricing;

public sealed class GetPlanPricingService(IPlanPricingResolver planPricingResolver)
    : IGetPlanPricingService
{
    public Task<Result<GetPlanPricingResult>> GetAsync(CancellationToken ct)
    {
        var items = planPricingResolver
            .GetAll()
            .Select(p => new PlanPricingResultItem(p.PlanCode.ToString(), p.Amount))
            .ToList();

        return Task.FromResult(Result<GetPlanPricingResult>.Success(new GetPlanPricingResult(items)));
    }
}