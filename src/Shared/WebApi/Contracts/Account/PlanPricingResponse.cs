namespace EduCrm.WebApi.Contracts.Account;

public sealed record PlanPricingResponse(IReadOnlyList<PlanPricingItemResponse> Items);

public sealed record PlanPricingItemResponse(string PlanCode, decimal Amount);