using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Repositories;

public sealed class OrganizationBillingDetailRepository : IOrganizationBillingDetailRepository
{
    private readonly AppDbContext _db;

    public OrganizationBillingDetailRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(OrganizationBillingDetail billingDetail)
    {
        _db.OrganizationBillingDetails.Add(billingDetail);
    }

    public Task<OrganizationBillingDetail?> GetByOrganizationIdAsync(Guid organizationId, CancellationToken ct)
    {
        return _db.OrganizationBillingDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.OrganizationId == organizationId, ct);
    }

    public Task<OrganizationBillingDetail?> GetTrackedByOrganizationIdAsync(Guid organizationId, CancellationToken ct)
    {
        return _db.OrganizationBillingDetails
            .FirstOrDefaultAsync(b => b.OrganizationId == organizationId, ct);
    }
}