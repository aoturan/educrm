using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetDashboardCounts;

public sealed class GetDashboardCountsService(
    IApplicationRepository applicationRepo,
    IProgramRepository programRepo,
    IFollowUpReader followUpReader,
    IOrgContext orgContext) : IGetDashboardCountsService
{
    public async Task<Result<GetDashboardCountsResult>> GetAsync(CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<GetDashboardCountsResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var organizationId = orgContext.OrganizationId.Value;

        var newApplicationsCount = await applicationRepo.CountNewAsync(organizationId, ct);
        var programsStartingCount = await programRepo.CountActiveStartingInNext7DaysAsync(organizationId, ct);
        var openFollowUpsCount = await followUpReader.CountOpenAsync(organizationId, ct);
        var overdueFollowUpsCount = await followUpReader.CountOpenAndOverdueAsync(organizationId, ct);

        return Result<GetDashboardCountsResult>.Success(new GetDashboardCountsResult(
            newApplicationsCount,
            programsStartingCount,
            openFollowUpsCount,
            overdueFollowUpsCount));
    }
}
