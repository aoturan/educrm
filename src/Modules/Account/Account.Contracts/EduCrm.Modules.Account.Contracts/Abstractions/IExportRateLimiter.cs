namespace EduCrm.Modules.Account.Contracts.Abstractions;

public interface IExportRateLimiter
{
    Task<bool> TryReserveSlotAsync(Guid userId, CancellationToken ct);
}
