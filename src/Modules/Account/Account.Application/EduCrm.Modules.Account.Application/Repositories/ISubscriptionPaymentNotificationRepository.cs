using EduCrm.Modules.Account.Domain.Entities;

namespace EduCrm.Modules.Account.Application.Repositories;

public interface ISubscriptionPaymentNotificationRepository
{
    void Add(SubscriptionPaymentNotification notification);
    Task<bool> ExistsBySubscriptionRequestIdAsync(Guid subscriptionRequestId, CancellationToken ct);
    Task<SubscriptionPaymentNotification?> GetBySubscriptionRequestIdAsync(Guid subscriptionRequestId, CancellationToken ct);
}
