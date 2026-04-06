using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.List;

public sealed class ListProgramsService(
    IProgramRepository programRepo,
    IOrgContext orgContext) : IListProgramsService
{
    public async Task<Result<ProgramListPagedResult>> ListAsync(ListProgramsInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<ProgramListPagedResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var queryResult = await programRepo.GetPagedListAsync(
            orgContext.OrganizationId.Value,
            input.Page,
            input.PageSize,
            ct,
            input.SearchTerm,
            input.PreFilter,
            input.ShowArchived,
            input.PersonId);

        return Result<ProgramListPagedResult>.Success(new ProgramListPagedResult(
            queryResult.Items.Select(x => new ProgramListResult(
                x.Id, x.Name, x.PublicShortDescription,
                x.Status, x.StartDate, x.EndDate, x.EnrollmentCount, x.IsArchived)).ToList(),
            input.Page,
            input.PageSize,
            queryResult.TotalCount));
    }
}
