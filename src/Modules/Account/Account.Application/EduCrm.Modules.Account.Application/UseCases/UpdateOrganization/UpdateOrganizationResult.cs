namespace EduCrm.Modules.Account.Application.UseCases.UpdateOrganization;

public sealed record UpdateOrganizationResult(
    Guid Id,
    string OrganizationName,
    string ContactName,
    string ContactEmail,
    string ContactPhone);
