using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.Repositories;

public interface IPlanPricingResolver
{
    decimal GetPrice(PlanCode planCode);
    IReadOnlyList<PlanPricingItem> GetAll();
}

public sealed record PlanPricingItem(PlanCode PlanCode, decimal Amount);