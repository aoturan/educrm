using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ExportPersons;

public sealed class ExportPersonsService(
    IPersonRepository personRepo,
    IPlanLimitsResolver planLimits,
    IExportRateLimiter rateLimiter,
    ICurrentUserSnapshot user,
    IClock clock) : IExportPersonsService
{
    private const int MaxRows = 1000;
    private const int RateLimitSeconds = 30;

    public async Task<Result<ExportPersonsResult>> ExportAsync(ExportPersonsInput input, CancellationToken ct)
    {
        var limits = await planLimits.ResolveAsync(user.OrganizationId, ct);
        if (!limits.ExportEnabled)
            return Result<ExportPersonsResult>.Fail(PeopleErrors.ExportNotAllowedOnPlan());

        var reserved = await rateLimiter.TryReserveSlotAsync(user.UserId, ct);
        if (!reserved)
            return Result<ExportPersonsResult>.Fail(PeopleErrors.ExportRateLimited(RateLimitSeconds));

        var rows = await personRepo.GetExportListAsync(
            user.OrganizationId,
            MaxRows + 1,
            ct,
            input.SearchTerm,
            input.PreFilter,
            input.ShowArchived);

        if (rows.Count > MaxRows)
            return Result<ExportPersonsResult>.Fail(PeopleErrors.ExportRowLimitExceeded(MaxRows));

        return Result<ExportPersonsResult>.Success(new ExportPersonsResult(rows, clock.UtcNow.UtcDateTime));
    }
}
