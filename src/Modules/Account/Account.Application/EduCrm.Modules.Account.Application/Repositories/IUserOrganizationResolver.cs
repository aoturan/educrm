namespace EduCrm.Modules.Account.Application.Repositories;

public interface IUserOrganizationResolver
{
    Task<Guid?> GetOrganizationIdAsync(Guid userId, CancellationToken ct);
}

