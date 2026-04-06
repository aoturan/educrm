using EduCrm.Modules.People.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ListPersons;

public sealed class ListPersonsService(
    IPersonRepository personRepo,
    IOrgContext orgContext) : IListPersonsService
{
    public async Task<Result<ListPersonsResult>> ListAsync(ListPersonsInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<ListPersonsResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var (items, totalCount, enrolledCount, notEnrolledCount) = await personRepo.GetPagedListAsync(
            orgContext.OrganizationId.Value,
            input.Page,
            input.PageSize,
            ct,
            input.SearchTerm,
            input.PreFilter,
            input.ShowArchived);

        return Result<ListPersonsResult>.Success(new ListPersonsResult(
            items,
            input.Page,
            input.PageSize,
            totalCount,
            enrolledCount,
            notEnrolledCount));
    }
}
