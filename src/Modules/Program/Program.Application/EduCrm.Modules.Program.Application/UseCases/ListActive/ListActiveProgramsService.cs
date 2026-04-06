using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ListActive;

public sealed class ListActiveProgramsService(
    IProgramRepository programRepo,
    IOrgContext orgContext) : IListActiveProgramsService
{
    public async Task<Result<IReadOnlyList<ListActiveProgramsResult>>> ListAsync(CancellationToken ct)
    {
        Console.WriteLine($"OrganizationId: {orgContext.OrganizationId}");
        if (orgContext.OrganizationId is null)
        {
            return Result<IReadOnlyList<ListActiveProgramsResult>>.Fail(
                CommonErrors.Forbidden("Organization scope is missing."));
        }

        var programs = await programRepo.GetActiveByOrganizationIdAsync(orgContext.OrganizationId.Value, ct);

        var result = programs
            .Select(x => new ListActiveProgramsResult(x.Id, x.Name))
            .ToList();

        return Result<IReadOnlyList<ListActiveProgramsResult>>.Success(result);
    }
}

