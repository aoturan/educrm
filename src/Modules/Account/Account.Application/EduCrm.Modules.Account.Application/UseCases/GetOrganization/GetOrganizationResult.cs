namespace EduCrm.Modules.Account.Application.UseCases.GetOrganization;

public sealed record GetOrganizationResult(
    Guid Id,
    string OrganizationName,
    string ContactName,
    string ContactEmail,
    string ContactPhone);