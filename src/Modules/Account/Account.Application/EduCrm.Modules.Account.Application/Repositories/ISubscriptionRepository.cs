using EduCrm.Modules.Account.Domain.Entities;

namespace EduCrm.Modules.Account.Application.Repositories;

public interface ISubscriptionRepository
{
    void Add(Subscription subscription);
    Task<Subscription?> GetCurrentByOrganizationAsync(Guid organizationId, CancellationToken ct);
    Task<Subscription?> GetCurrentTrackedByOrganizationAsync(Guid organizationId, CancellationToken ct);
    Task<int> CountActivePaidAsync(DateTime nowUtc, CancellationToken ct);
    Task<int> CountFreeOrExpiredAsync(DateTime nowUtc, CancellationToken ct);
}