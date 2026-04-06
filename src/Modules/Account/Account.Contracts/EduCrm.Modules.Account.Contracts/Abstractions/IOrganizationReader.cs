using EduCrm.Modules.Account.Contracts.Dtos;

namespace EduCrm.Modules.Account.Contracts.Abstractions;

public interface IOrganizationReader
{
    Task<OrganizationSubscriptionInfo?> GetSubscriptionInfoAsync(Guid organizationId, CancellationToken ct);
}