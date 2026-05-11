using EduCrm.Modules.Account.Application.Repositories.Models;
using EduCrm.Modules.Account.Domain.Entities;

namespace EduCrm.Modules.Account.Application.Repositories;

public interface IOrganizationRepository
{
    void Add(Organization organization);
    Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<int> CountCreatedSinceAsync(DateTime sinceUtc, CancellationToken ct);

    Task<OrganizationPagedListQueryResult> GetPagedListAsync(
        int page,
        int pageSize,
        DateTime nowUtc,
        CancellationToken ct,
        string? searchTerm = null,
        string? face = null);
}
