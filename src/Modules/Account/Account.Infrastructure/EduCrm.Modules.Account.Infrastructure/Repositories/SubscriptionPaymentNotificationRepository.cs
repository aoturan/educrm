using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Account.Infrastructure.Repositories;

public sealed class SubscriptionPaymentNotificationRepository : ISubscriptionPaymentNotificationRepository
{
    private readonly AppDbContext _db;

    public SubscriptionPaymentNotificationRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(SubscriptionPaymentNotification notification)
    {
        _db.SubscriptionPaymentNotifications.Add(notification);
    }

    public Task<bool> ExistsBySubscriptionRequestIdAsync(Guid subscriptionRequestId, CancellationToken ct)
    {
        return _db.SubscriptionPaymentNotifications
            .AsNoTracking()
            .AnyAsync(n => n.SubscriptionRequestId == subscriptionRequestId, ct);
    }

    public Task<SubscriptionPaymentNotification?> GetBySubscriptionRequestIdAsync(Guid subscriptionRequestId, CancellationToken ct)
    {
        return _db.SubscriptionPaymentNotifications
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.SubscriptionRequestId == subscriptionRequestId, ct);
    }
}
