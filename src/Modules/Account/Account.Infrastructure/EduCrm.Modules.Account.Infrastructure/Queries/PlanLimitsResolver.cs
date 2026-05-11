using EduCrm.Infrastructure.Data;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Account.Contracts.Dtos;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EduCrm.Modules.Account.Infrastructure.Queries;

public sealed class PlanLimitsResolver : IPlanLimitsResolver
{
    private readonly AppDbContext _db;
    private readonly IOptionsSnapshot<PlanLimitsOptions> _options;
    private readonly IClock _clock;

    public PlanLimitsResolver(AppDbContext db, IOptionsSnapshot<PlanLimitsOptions> options, IClock clock)
    {
        _db = db;
        _options = options;
        _clock = clock;
    }

    public async Task<PlanLimits> ResolveAsync(Guid organizationId, CancellationToken ct)
    {
        var row = await _db.Subscriptions
            .AsNoTracking()
            .Where(s => s.OrganizationId == organizationId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .Select(s => new { s.PlanCode, s.EndsAtUtc })
            .FirstOrDefaultAsync(ct);

        if (row is null)
            return _options.Value.Free;

        var now = _clock.UtcNow.UtcDateTime;
        if (row.EndsAtUtc is { } endsAt && endsAt < now)
            return _options.Value.Free;

        return row.PlanCode switch
        {
            PlanCode.Plus => _options.Value.Plus,
            PlanCode.Pro  => _options.Value.Pro,
            _             => _options.Value.Free,
        };
    }
}