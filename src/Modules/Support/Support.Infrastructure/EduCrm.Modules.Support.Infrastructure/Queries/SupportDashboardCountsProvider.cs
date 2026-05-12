using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Support.Contracts.Abstractions;
using EduCrm.Modules.Support.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Support.Infrastructure.Queries;

public sealed class SupportDashboardCountsProvider(AppDbContext db) : ISupportDashboardCountsProvider
{
    public Task<int> CountNewContactMessagesAsync(CancellationToken ct)
    {
        return db.SupportContactMessages
            .AsNoTracking()
            .CountAsync(x => x.Status == SupportContactMessageStatus.New, ct);
    }

    public Task<int> CountNewRequestsAsync(CancellationToken ct)
    {
        return db.SupportRequests
            .AsNoTracking()
            .CountAsync(x => x.Status == SupportRequestStatus.New, ct);
    }
}
