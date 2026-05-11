namespace EduCrm.Modules.Account.Application.UseCases.GetBillingDetail;

public sealed record GetBillingDetailResult(
    string BillingType,
    string BillingName,
    string? TaxNumber,
    string? TaxOffice,
    string BillingEmail,
    string BillingAddress);