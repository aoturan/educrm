using EduCrm.Modules.Account.Contracts.Dtos;

namespace EduCrm.Modules.Account.Contracts.Abstractions;

public interface IPlanUsageResolver
{
    Task<PlanUsageSnapshot> ResolveAsync(Guid organizationId, CancellationToken ct);
}