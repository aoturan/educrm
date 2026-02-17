namespace EduCrm.Infrastructure.Persistence;

public interface IAppDbTransaction
{
    Task<IAppDbTransactionScope> BeginAsync(CancellationToken ct);
}

public interface IAppDbTransactionScope : IAsyncDisposable
{
    Task CommitAsync(CancellationToken ct);
    Task RollbackAsync(CancellationToken ct);
}