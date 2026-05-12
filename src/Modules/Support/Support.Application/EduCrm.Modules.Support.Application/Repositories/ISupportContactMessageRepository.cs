using EduCrm.Modules.Support.Application.Repositories.Models;
using EduCrm.Modules.Support.Domain.Entities;

namespace EduCrm.Modules.Support.Application.Repositories;

public interface ISupportContactMessageRepository
{
    void Add(SupportContactMessage supportContactMessage);

    Task<SupportContactMessage?> GetTrackedByIdAsync(Guid id, CancellationToken ct);

    Task<SupportContactMessagePagedListQueryResult> GetPagedListAsync(
        int page,
        int pageSize,
        CancellationToken ct,
        string? preFilter = null);
}
