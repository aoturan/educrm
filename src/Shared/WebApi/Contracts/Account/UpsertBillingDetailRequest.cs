using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Account;

public sealed record UpsertBillingDetailRequest(
    BillingType BillingType,
    string BillingName,
    string TaxNumber,
    string? TaxOffice,
    string BillingEmail,
    string BillingAddress);