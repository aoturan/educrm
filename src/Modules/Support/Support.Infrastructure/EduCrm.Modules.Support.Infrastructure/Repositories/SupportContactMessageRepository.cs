using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.Modules.Support.Application.Repositories.Models;
using EduCrm.Modules.Support.Domain.Entities;
using EduCrm.Modules.Support.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.Support.Infrastructure.Repositories;

public sealed class SupportContactMessageRepository(AppDbContext db) : ISupportContactMessageRepository
{
    public void Add(SupportContactMessage supportContactMessage)
    {
        db.SupportContactMessages.Add(supportContactMessage);
    }

    public Task<SupportContactMessage?> GetTrackedByIdAsync(Guid id, CancellationToken ct)
    {
        return db.SupportContactMessages.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<SupportContactMessagePagedListQueryResult> GetPagedListAsync(
        int page,
        int pageSize,
        CancellationToken ct,
        string? preFilter = null)
    {
        var baseQuery = db.SupportContactMessages.AsNoTracking();

        var filteredQuery = preFilter switch
        {
            "new" => baseQuery.Where(x => x.Status == SupportContactMessageStatus.New),
            "handled" => baseQuery.Where(x => x.Status == SupportContactMessageStatus.Handled),
            _ => baseQuery
        };

        var totalCount = await filteredQuery.CountAsync(ct);

        var items = await filteredQuery
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SupportContactMessageListItem(
                x.Id,
                x.FullName,
                x.Email,
                x.Subject,
                x.Message,
                x.Status,
                x.CreatedAt,
                x.ReviewedAt))
            .ToListAsync(ct);

        return new SupportContactMessagePagedListQueryResult(items, totalCount);
    }
}
