using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetApplicationById;

public sealed class GetApplicationByIdService(
    IApplicationRepository applicationRepo,
    IOrgContext orgContext) : IGetApplicationByIdService
{
    public async Task<Result<GetApplicationByIdResult>> GetAsync(Guid applicationId, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<GetApplicationByIdResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var detail = await applicationRepo.GetDetailAsync(applicationId, orgContext.OrganizationId.Value, ct);
        if (detail is null)
            return Result<GetApplicationByIdResult>.Fail(ProgramErrors.ApplicationNotFound(applicationId));

        var person = detail.PersonId.HasValue
            ? new ApplicationPersonInfo(detail.PersonId.Value, detail.PersonFullName!)
            : null;

        var program = new ApplicationProgramInfo(detail.ProgramId, detail.ProgramName);

        return Result<GetApplicationByIdResult>.Success(new GetApplicationByIdResult(
            detail.Id,
            detail.Status,
            detail.SubmittedFullName,
            detail.SubmittedPhone,
            detail.SubmittedMessage,
            detail.FirstSubmittedAtUtc,
            detail.LastSubmittedAtUtc,
            detail.SubmissionCount,
            detail.ConvertedAtUtc,
            detail.ClosedAtUtc,
            detail.ClosedNote,
            person,
            program));
    }
}

