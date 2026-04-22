namespace EduCrm.Modules.People.Contracts.Abstractions;

public interface IFollowUpReader
{
    Task<int> CountOpenAsync(Guid organizationId, CancellationToken ct);
    Task<int> CountOpenAndOverdueAsync(Guid organizationId, CancellationToken ct);
}

