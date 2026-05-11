using EduCrm.Modules.Account.Application.Repositories.Models;
using EduCrm.Modules.Account.Domain.Entities;

namespace EduCrm.Modules.Account.Application.Repositories;

public interface ISubscriptionRequestRepository
{
    void Add(SubscriptionRequest request);
    Task<SubscriptionRequest?> GetByIdAsync(Guid requestId, CancellationToken ct);
    Task<SubscriptionRequest?> GetTrackedByIdAsync(Guid requestId, CancellationToken ct);
    Task<SubscriptionRequest?> GetActiveByOrganizationAsync(Guid organizationId, CancellationToken ct);
    Task<bool> ExistsByReferenceCodeAsync(string referenceCode, CancellationToken ct);
    Task<int> CountPendingAsync(CancellationToken ct);
    Task<IReadOnlyList<PendingPaymentRequestSummary>> GetOldestPendingAsync(int take, CancellationToken ct);
    Task<IReadOnlyList<OrganizationSubscriptionRequestData>> GetByOrganizationAsync(Guid organizationId, CancellationToken ct);

    Task<SubscriptionRequestPagedListQueryResult> GetPagedListAsync(
        int page,
        int pageSize,
        CancellationToken ct,
        string? searchTerm = null,
        string? face = null);
}
