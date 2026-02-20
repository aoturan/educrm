using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Application.Repositories.Models;

namespace EduCrm.Modules.Account.Application.Repositories;

public interface IUserRepository
{
    void Add(User user);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken ct);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<UserWithOrganization?> GetByEmailWithOrganizationAsync(string email, CancellationToken ct);
    Task<UserWithOrganization?> GetByIdWithOrganizationAsync(Guid userId, CancellationToken ct);
}