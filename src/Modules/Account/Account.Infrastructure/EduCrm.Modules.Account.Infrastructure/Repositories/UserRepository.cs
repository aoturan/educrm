using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Repositories.Models;
using EduCrm.Modules.Account.Domain.Entities;
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
                    u.PasswordHash,
                    u.Email,
                    u.FullName,
                    o.Name
                )
            )
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);
    }
}