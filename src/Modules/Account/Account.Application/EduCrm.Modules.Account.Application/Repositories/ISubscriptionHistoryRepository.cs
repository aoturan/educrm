using EduCrm.Modules.Account.Domain.Entities;

namespace EduCrm.Modules.Account.Application.Repositories;

public interface ISubscriptionHistoryRepository
{
    void Add(SubscriptionHistory entry);
}