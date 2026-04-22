using EduCrm.Infrastructure.Data;
using EduCrm.Modules.People.Contracts.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.People.Infrastructure.Readers;

public sealed class PersonReader(AppDbContext db) : IPersonReader
{
    public async Task<IReadOnlyList<PersonContactMatch>> FindByContactAsync(
        string email,
        string phone,
        Guid organizationId,
        CancellationToken ct)
    {
        return await db.Persons
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId
                     && !p.IsArchived
                     && (p.Email == email || p.Phone == phone))
            .Select(p => new PersonContactMatch(p.Id, p.FullName, p.Email, p.Phone))
            .ToListAsync(ct);
    }

    public Task<bool> ExistsInOrganizationAsync(Guid personId, Guid organizationId, CancellationToken ct)
    {
        return db.Persons
            .AnyAsync(p => p.Id == personId && p.OrganizationId == organizationId && !p.IsArchived, ct);
    }

    public Task<PersonContactMatch?> GetByIdAsync(Guid personId, Guid organizationId, CancellationToken ct)
    {
        return db.Persons
            .AsNoTracking()
            .Where(p => p.Id == personId && p.OrganizationId == organizationId && !p.IsArchived)
            .Select(p => new PersonContactMatch(p.Id, p.FullName, p.Email, p.Phone))
            .FirstOrDefaultAsync(ct);
    }
}
