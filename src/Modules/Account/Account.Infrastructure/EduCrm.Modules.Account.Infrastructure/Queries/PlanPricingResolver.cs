using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Contracts.Dtos;
using EduCrm.Modules.Account.Domain.Enums;
using Microsoft.Extensions.Options;

namespace EduCrm.Modules.Account.Infrastructure.Queries;

public sealed class PlanPricingResolver : IPlanPricingResolver
{
    private readonly IOptionsSnapshot<PlanPricingOptions> _options;

    public PlanPricingResolver(IOptionsSnapshot<PlanPricingOptions> options)
    {
        _options = options;
    }

    public decimal GetPrice(PlanCode planCode) => planCode switch
    {
        PlanCode.Free => _options.Value.Free,
        PlanCode.Plus => _options.Value.Plus,
        PlanCode.Pro  => _options.Value.Pro,
        _             => throw new ArgumentOutOfRangeException(nameof(planCode), planCode, "Unknown plan code.")
    };

    public IReadOnlyList<PlanPricingItem> GetAll() => new[]
    {
        new PlanPricingItem(PlanCode.Free, _options.Value.Free),
        new PlanPricingItem(PlanCode.Plus, _options.Value.Plus),
        new PlanPricingItem(PlanCode.Pro,  _options.Value.Pro)
    };
}