using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ExportApplications;

public sealed class ExportApplicationsService(
    IApplicationRepository applicationRepo,
    IProgramRepository programRepo,
    IPlanLimitsResolver planLimits,
    IExportRateLimiter rateLimiter,
    IOrgContext orgContext,
    ICurrentUser currentUser,
    IClock clock) : IExportApplicationsService
{
    private const int MaxRows = 1000;
    private const int RateLimitSeconds = 30;

    public async Task<Result<ExportApplicationsResult>> ExportAsync(ExportApplicationsInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<ExportApplicationsResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        if (currentUser.UserId is null)
            return Result<ExportApplicationsResult>.Fail(CommonErrors.Forbidden("Authenticated user is required."));

        var organizationId = orgContext.OrganizationId.Value;

        if (input.ProgramId.HasValue)
        {
            var programExists = await programRepo.ExistsAsync(input.ProgramId.Value, organizationId, ct);
            if (!programExists)
                return Result<ExportApplicationsResult>.Fail(ProgramErrors.ProgramNotFound(input.ProgramId.Value));
        }

        var limits = await planLimits.ResolveAsync(organizationId, ct);
        if (!limits.ExportEnabled)
            return Result<ExportApplicationsResult>.Fail(ProgramErrors.ExportNotAllowedOnPlan());

        var reserved = await rateLimiter.TryReserveSlotAsync(currentUser.UserId.Value, ct);
        if (!reserved)
            return Result<ExportApplicationsResult>.Fail(ProgramErrors.ExportRateLimited(RateLimitSeconds));

        var rows = await applicationRepo.GetExportListAsync(
            organizationId,
            MaxRows + 1,
            ct,
            input.Statuses,
            input.ProgramId);

        if (rows.Count > MaxRows)
            return Result<ExportApplicationsResult>.Fail(ProgramErrors.ExportRowLimitExceeded(MaxRows));

        return Result<ExportApplicationsResult>.Success(new ExportApplicationsResult(rows, clock.UtcNow.UtcDateTime));
    }
}
