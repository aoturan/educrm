namespace EduCrm.Modules.Account.Application.UseCases.UpdateOrganization;

public sealed record UpdateOrganizationInput(
    Guid CallerUserId,
    Guid CallerOrganizationId,
    string OrganizationName,
    string ContactName,
    string ContactEmail,
    string ContactPhone);
