using EduCrm.Modules.Account.Domain.Entities;

namespace EduCrm.Modules.Account.Application.Repositories;

public interface IOrganizationBillingDetailRepository
{
    void Add(OrganizationBillingDetail billingDetail);
    Task<OrganizationBillingDetail?> GetByOrganizationIdAsync(Guid organizationId, CancellationToken ct);
    Task<OrganizationBillingDetail?> GetTrackedByOrganizationIdAsync(Guid organizationId, CancellationToken ct);
}