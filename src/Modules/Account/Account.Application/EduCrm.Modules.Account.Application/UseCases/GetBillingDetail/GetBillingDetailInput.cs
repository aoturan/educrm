namespace EduCrm.Modules.Account.Application.UseCases.GetBillingDetail;

public sealed record GetBillingDetailInput(
    Guid CallerUserId,
    Guid CallerOrganizationId);