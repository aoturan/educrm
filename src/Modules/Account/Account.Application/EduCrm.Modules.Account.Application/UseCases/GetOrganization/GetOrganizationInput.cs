namespace EduCrm.Modules.Account.Application.UseCases.GetOrganization;

public sealed record GetOrganizationInput(
    Guid CallerUserId,
    Guid CallerOrganizationId);