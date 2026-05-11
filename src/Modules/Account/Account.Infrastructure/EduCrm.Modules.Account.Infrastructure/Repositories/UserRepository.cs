using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Repositories.Models;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(User user)
    {
        _db.Users.Add(user);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken ct)
    {
        return await _db.Users
            .FirstOrDefaultAsync(u => u.Id == userId, ct);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await _db.Users
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<bool> ExistsByEmailInOrganizationAsync(string email, Guid organizationId, CancellationToken ct)
    {
        return await _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email && u.OrganizationId == organizationId, ct);
    }

    public async Task<UserWithOrganization?> GetByEmailWithOrganizationAsync(string email, CancellationToken ct)
    {
        return await (
                from u in _db.Users
                where u.Email == email
                join o in _db.Organizations on u.OrganizationId equals o.Id
                select new UserWithOrganization(
                    u.Id,
                    u.OrganizationId,
                    u.Status,
                    u.Role,
                    u.IsApplicationAdmin,
                    u.PasswordHash,
                    u.Email,
                    u.FullName,
                    o.Name
                )
            )
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);
    }

    public async Task<UserWithOrganization?> GetByIdWithOrganizationAsync(Guid userId, CancellationToken ct)
    {
        return await (
                from u in _db.Users
                where u.Id == userId
                join o in _db.Organizations on u.OrganizationId equals o.Id
                select new UserWithOrganization(
                    u.Id,
                    u.OrganizationId,
                    u.Status,
                    u.Role,
                    u.IsApplicationAdmin,
                    u.PasswordHash,
                    u.Email,
                    u.FullName,
                    o.Name
                )
            )
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);
    }

    public Task<int> CountActiveByOrganizationAsync(Guid organizationId, CancellationToken ct)
    {
        return _db.Users
            .AsNoTracking()
            .CountAsync(u => u.OrganizationId == organizationId
                          && u.Status == UserStatus.Active, ct);
    }

    public async Task<UserPagedListQueryResult> GetPagedListByOrganizationAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken ct,
        IReadOnlyCollection<Domain.Enums.UserStatus>? statuses = null)
    {
        var baseQuery = _db.Users
            .AsNoTracking()
            .Where(u => u.OrganizationId == organizationId);

        if (statuses is { Count: > 0 })
        {
            baseQuery = baseQuery.Where(u => statuses.Contains(u.Status));
        }

        var totalCount = await baseQuery.CountAsync(ct);

        var items = await baseQuery
            .OrderByDescending(u => u.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserListItem(
                u.Id,
                u.FullName,
                u.Email,
                u.Role,
                u.Status,
                u.LastLoginAtUtc))
            .ToListAsync(ct);

        return new UserPagedListQueryResult(items, totalCount);
    }

    public Task<OrganizationOwnerData?> GetOrganizationOwnerAsync(Guid organizationId, CancellationToken ct)
    {
        return _db.Users
            .AsNoTracking()
            .Where(u => u.OrganizationId == organizationId && u.Role == UserRole.Admin)
            .OrderBy(u => u.CreatedAtUtc)
            .Select(u => new OrganizationOwnerData(u.Id, u.FullName, u.Email, u.CreatedAtUtc))
            .FirstOrDefaultAsync(ct);
    }
}
