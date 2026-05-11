namespace EduCrm.WebApi.Contracts.Account;

public sealed record UpdateOrganizationRequest(
    string OrganizationName,
    string ContactName,
    string ContactEmail,
    string ContactPhone);
