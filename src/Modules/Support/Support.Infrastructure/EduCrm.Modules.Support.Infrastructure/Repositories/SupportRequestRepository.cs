using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.Modules.Support.Domain.Entities;

namespace EduCrm.Modules.Support.Infrastructure.Repositories;

public sealed class SupportRequestRepository(AppDbContext db) : ISupportRequestRepository
{
    public void Add(SupportRequest supportRequest)
    {
        db.SupportRequests.Add(supportRequest);
    }
}

