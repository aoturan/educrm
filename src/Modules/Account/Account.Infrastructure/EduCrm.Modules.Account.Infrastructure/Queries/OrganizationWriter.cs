using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Contracts.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Queries;

public sealed class OrganizationWriter(AppDbContext db) : IOrganizationWriter
{
    public async Task<bool> UpdateFreeProgramConsumedAtUtcAsync(Guid organizationId, DateTime nowUtc, CancellationToken ct)
    {
        var organization = await db.Organizations.FirstOrDefaultAsync(o => o.Id == organizationId, ct);
        if (organization is null)
        {
            return false;
        }

        organization.MarkFreeProgramConsumed(nowUtc);
        return true;
    }
}

