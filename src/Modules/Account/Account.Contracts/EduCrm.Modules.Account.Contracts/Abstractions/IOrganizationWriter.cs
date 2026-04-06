namespace EduCrm.Modules.Account.Contracts.Abstractions;

public interface IOrganizationWriter
{
    Task<bool> UpdateFreeProgramConsumedAtUtcAsync(Guid organizationId, DateTime nowUtc, CancellationToken ct);
}

