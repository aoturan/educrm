using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Repositories;

public sealed class UserOrganizationResolver : IUserOrganizationResolver
{
    private readonly AppDbContext _db;

    public UserOrganizationResolver(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid?> GetOrganizationIdAsync(Guid userId, CancellationToken ct)
    {
        return await _db.Users
            .Where(u => u.Id == userId)
            .Select(u => (Guid?)u.OrganizationId)
            .FirstOrDefaultAsync(ct);
    }
}

