namespace EduCrm.WebApi.Contracts.Account;

public sealed record BillingDetailResponse(
    string BillingType,
    string BillingName,
    string? TaxNumber,
    string? TaxOffice,
    string BillingEmail,
    string BillingAddress);