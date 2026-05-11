using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;

namespace EduCrm.Modules.Account.Infrastructure.Repositories;

public sealed class SubscriptionHistoryRepository : ISubscriptionHistoryRepository
{
    private readonly AppDbContext _db;

    public SubscriptionHistoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(SubscriptionHistory entry)
    {
        _db.SubscriptionHistories.Add(entry);
    }
}