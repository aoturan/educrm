using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.DeleteEnrollment;

public sealed class DeleteEnrollmentService(
    IEnrollmentRepository enrollmentRepo,
    IOrgContext orgContext) : IDeleteEnrollmentService
{
    public async Task<Result> DeleteAsync(Guid enrollmentId, CancellationToken ct)
    {
        
        if (orgContext.OrganizationId is null)
        {
            return Result.Fail(CommonErrors.Forbidden("Organization scope is missing."));
        }

        var programStatus = await enrollmentRepo.GetProgramStatusByEnrollmentIdAsync(
            enrollmentId,
            orgContext.OrganizationId.Value,
            ct);

        if (programStatus is null)
        {
            return Result.Fail(ProgramErrors.EnrollmentNotFound(enrollmentId));
        }

        if (programStatus != ProgramStatus.Active)
        {
            return Result.Fail(ProgramErrors.EnrollmentDeleteNotAllowed());
        }

        await enrollmentRepo.DeleteAsync(enrollmentId, orgContext.OrganizationId.Value, ct);

        return Result.Success();
    }
}
