using EduCrm.Modules.Account.Contracts.Dtos;

namespace EduCrm.Modules.Account.Contracts.Abstractions;

public interface IPlanLimitsResolver
{
    Task<PlanLimits> ResolveAsync(Guid organizationId, CancellationToken ct);
}