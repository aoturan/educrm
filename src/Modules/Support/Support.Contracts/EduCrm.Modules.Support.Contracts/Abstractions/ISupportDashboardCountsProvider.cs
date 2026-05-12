namespace EduCrm.Modules.Support.Contracts.Abstractions;

public interface ISupportDashboardCountsProvider
{
    Task<int> CountNewContactMessagesAsync(CancellationToken ct);

    Task<int> CountNewRequestsAsync(CancellationToken ct);
}
