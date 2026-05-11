using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.UpsertBillingDetail;

public sealed record UpsertBillingDetailInput(
    Guid CallerUserId,
    Guid CallerOrganizationId,
    BillingType BillingType,
    string BillingName,
    string? TaxNumber,
    string? TaxOffice,
    string BillingEmail,
    string BillingAddress);