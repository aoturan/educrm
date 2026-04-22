using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ListApplications;

public sealed class ListApplicationsService(
    IApplicationRepository applicationRepo,
    IProgramRepository programRepo,
    IOrgContext orgContext) : IListApplicationsService
{
    public async Task<Result<ListApplicationsResult>> ListAsync(ListApplicationsInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<ListApplicationsResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var organizationId = orgContext.OrganizationId.Value;

        if (input.ProgramId.HasValue)
        {
            var programExists = await programRepo.ExistsAsync(input.ProgramId.Value, organizationId, ct);
            if (!programExists)
                return Result<ListApplicationsResult>.Fail(ProgramErrors.ProgramNotFound(input.ProgramId.Value));
        }

        var page = input.IsBrief ? 1 : input.Page;
        var pageSize = input.IsBrief ? 5 : input.PageSize;

        var queryResult = await applicationRepo.GetPagedListAsync(
            organizationId,
            page,
            pageSize,
            ct,
            input.Statuses,
            input.ProgramId);

        return Result<ListApplicationsResult>.Success(new ListApplicationsResult(
            queryResult.Items,
            page,
            pageSize,
            queryResult.TotalCount));
    }
}
