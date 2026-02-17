using EduCrm.Modules.Account.Domain.Entities;

namespace EduCrm.Modules.Account.Application.Repositories;

public interface IOrganizationRepository
{
    void Add(Organization organization);
    Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct);
}