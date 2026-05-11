namespace EduCrm.Modules.Account.Application.UseCases.GetPlanPricing;

public sealed record GetPlanPricingResult(IReadOnlyList<PlanPricingResultItem> Items);

public sealed record PlanPricingResultItem(string PlanCode, decimal Amount);