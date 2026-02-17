using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Repositories;

public sealed class OrganizationRepository : IOrganizationRepository
{
    private readonly AppDbContext _db;

    public OrganizationRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(Organization organization)
    {
        _db.Organizations.Add(organization);
    }

    public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Organizations
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }
}