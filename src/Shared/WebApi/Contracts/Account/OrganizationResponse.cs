namespace EduCrm.WebApi.Contracts.Account;

public sealed record OrganizationResponse(
    Guid Id,
    string OrganizationName,
    string ContactName,
    string ContactEmail,
    string ContactPhone);