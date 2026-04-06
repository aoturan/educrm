using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetEnrollmentCandidates;

public sealed class GetEnrollmentCandidatesService(
    IEnrollmentRepository enrollmentRepo,
    IOrgContext orgContext) : IGetEnrollmentCandidatesService
{
    public async Task<Result<GetEnrollmentCandidatesResult>> GetAsync(
        GetEnrollmentCandidatesInput input,
        CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
        {
            return Result<GetEnrollmentCandidatesResult>.Fail(
                CommonErrors.Forbidden("Organization scope is missing."));
        }

        var search = string.IsNullOrWhiteSpace(input.Search) ? null : input.Search.Trim();

        var (items, totalCount) = await enrollmentRepo.GetCandidatesAsync(
            input.ProgramId,
            orgContext.OrganizationId.Value,
            search,
            input.Page,
            input.PageSize,
            ct);

        var result = new GetEnrollmentCandidatesResult(
            items.Select(x => new EnrollmentCandidateItem(x.PersonId, x.FullName, x.Phone, x.Email)).ToList(),
            input.Page,
            input.PageSize,
            totalCount);

        return Result<GetEnrollmentCandidatesResult>.Success(result);
    }
}

