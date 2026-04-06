using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Domain.Entities;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.CreateEnrollment;

public sealed class CreateEnrollmentService(
    IEnrollmentRepository enrollmentRepo,
    IProgramRepository programRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext) : ICreateEnrollmentService
{
    public async Task<Result<CreateEnrollmentResult>> CreateAsync(
        CreateEnrollmentInput input,
        CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
        {
            return Result<CreateEnrollmentResult>.Fail(
                CommonErrors.Forbidden("Organization scope is missing."));
        }

        var organizationId = orgContext.OrganizationId.Value;

        var programExists = await programRepo.ExistsAsync(input.ProgramId, organizationId, ct);
        if (!programExists)
        {
            return Result<CreateEnrollmentResult>.Fail(ProgramErrors.ProgramNotFound(input.ProgramId));
        }

        var personExists = await enrollmentRepo.PersonExistsInOrgAsync(input.PersonId, organizationId, ct);
        if (!personExists)
        {
            return Result<CreateEnrollmentResult>.Fail(ProgramErrors.PersonNotFound(input.PersonId));
        }

        var alreadyEnrolled = await enrollmentRepo.ExistsAsync(input.ProgramId, input.PersonId, organizationId, ct);
        if (alreadyEnrolled)
        {
            return Result<CreateEnrollmentResult>.Fail(ProgramErrors.AlreadyEnrolled());
        }

        var enrollment = new Enrollment(
            organizationId,
            input.ProgramId,
            input.PersonId,
            clock.UtcNow.UtcDateTime);

        enrollmentRepo.Add(enrollment);
        await uow.SaveChangesAsync(ct);

        return Result<CreateEnrollmentResult>.Success(new CreateEnrollmentResult(enrollment.Id));
    }
}

