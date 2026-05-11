namespace EduCrm.Modules.Account.Application.Repositories.Models;

public sealed record OrganizationOwnerData(
    Guid UserId,
    string FullName,
    string Email,
    DateTime CreatedAtUtc);
