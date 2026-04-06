using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.Search;

public sealed class SearchProgramsService(
    IProgramRepository programRepo,
    IOrgContext orgContext) : ISearchProgramsService
{
    public async Task<Result<IReadOnlyList<SearchProgramsResult>>> SearchAsync(string nameQuery, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
        {
            return Result<IReadOnlyList<SearchProgramsResult>>.Fail(
                CommonErrors.Forbidden("Organization scope is missing."));
        }

        var items = await programRepo.GetAllByOrganizationIdAsync(
            orgContext.OrganizationId.Value,
            ct,
            nameQuery);

        var results = items
            .Select(x => new SearchProgramsResult(
                x.Id,
                x.Name,
                x.PublicShortDescription,
                x.Status,
                x.StartDate,
                x.EndDate,
                x.EnrollmentCount,
                x.IsArchived))
            .ToList();

        return Result<IReadOnlyList<SearchProgramsResult>>.Success(results);
    }
}

