using EduCrm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace EduCrm.Infrastructure.Data;

public sealed class AppDbTransaction : IAppDbTransaction
{
    private readonly AppDbContext _db;

    public AppDbTransaction(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IAppDbTransactionScope> BeginAsync(CancellationToken ct)
    {
        var trx = await _db.Database.BeginTransactionAsync(ct);
        return new Scope(trx);
    }

    private sealed class Scope : IAppDbTransactionScope
    {
        private readonly IDbContextTransaction _trx;

        public Scope(IDbContextTransaction trx)
        {
            _trx = trx;
        }

        public Task CommitAsync(CancellationToken ct)
            => _trx.CommitAsync(ct);

        public Task RollbackAsync(CancellationToken ct)
            => _trx.RollbackAsync(ct);

        public ValueTask DisposeAsync()
            => _trx.DisposeAsync();
    }
}