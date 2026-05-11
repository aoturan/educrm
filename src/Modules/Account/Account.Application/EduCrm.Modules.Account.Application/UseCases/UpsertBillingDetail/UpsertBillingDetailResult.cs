namespace EduCrm.Modules.Account.Application.UseCases.UpsertBillingDetail;

public sealed record UpsertBillingDetailResult(
    string BillingType,
    string BillingName,
    string? TaxNumber,
    string? TaxOffice,
    string BillingEmail,
    string BillingAddress);