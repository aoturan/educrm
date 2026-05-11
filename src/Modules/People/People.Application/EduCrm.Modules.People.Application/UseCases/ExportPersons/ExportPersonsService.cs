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
    IOrgContext orgContext,
    ICurrentUser currentUser,
    IClock clock) : IExportPersonsService
{
    private const int MaxRows = 1000;
    private const int RateLimitSeconds = 30;

    public async Task<Result<ExportPersonsResult>> ExportAsync(ExportPersonsInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<ExportPersonsResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        if (currentUser.UserId is null)
            return Result<ExportPersonsResult>.Fail(CommonErrors.Forbidden("Authenticated user is required."));

        var limits = await planLimits.ResolveAsync(orgContext.OrganizationId.Value, ct);
        if (!limits.ExportEnabled)
            return Result<ExportPersonsResult>.Fail(PeopleErrors.ExportNotAllowedOnPlan());

        var reserved = await rateLimiter.TryReserveSlotAsync(currentUser.UserId.Value, ct);
        if (!reserved)
            return Result<ExportPersonsResult>.Fail(PeopleErrors.ExportRateLimited(RateLimitSeconds));

        var rows = await personRepo.GetExportListAsync(
            orgContext.OrganizationId.Value,
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
