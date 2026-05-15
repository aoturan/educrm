namespace EduCrm.Modules.Account.Application.UseCases.UpdateOrganization;

public sealed record UpdateOrganizationInput(
    string OrganizationName,
    string ContactName,
    string ContactEmail,
    string ContactPhone);
