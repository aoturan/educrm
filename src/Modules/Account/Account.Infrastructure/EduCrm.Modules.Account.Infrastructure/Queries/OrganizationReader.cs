using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Account.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Queries;

public class OrganizationReader: IOrganizationReader
{
    private readonly AppDbContext _db;
    
    public OrganizationReader(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<OrganizationSubscriptionInfo?> GetSubscriptionInfoAsync(Guid organizationId, CancellationToken ct)
    {
        var subscriptionInfo = await _db.Organizations
            .Where(o => o.Id == organizationId)
            .Select(o => new OrganizationSubscriptionInfo(
                o.Id,
                o.PlanType,
                o.SubscriptionBillingCycle,
                o.SubscriptionStatus,
                o.SubscriptionEndsAtUtc,
                o.FreeProgramConsumedAtUtc
            )).FirstOrDefaultAsync(ct);

        return subscriptionInfo;
    }
}