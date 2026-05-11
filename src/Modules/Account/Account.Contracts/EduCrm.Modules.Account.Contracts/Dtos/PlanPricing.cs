namespace EduCrm.Modules.Account.Contracts.Dtos;

public sealed class PlanPricingOptions
{
    public decimal Free { get; init; }
    public decimal Plus { get; init; }
    public decimal Pro  { get; init; }
}