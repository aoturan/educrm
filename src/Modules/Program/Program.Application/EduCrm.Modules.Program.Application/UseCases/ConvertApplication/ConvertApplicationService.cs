using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Domain.Entities;
using EduCrm.Modules.Program.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ConvertApplication;

public sealed class ConvertApplicationService(
    IApplicationRepository applicationRepo,
    IEnrollmentRepository enrollmentRepo,
    IPersonReader personReader,
    IUnitOfWork uow,
    IOrgContext orgContext,
    IClock clock) : IConvertApplicationService
{
    public async Task<Result> ConvertAsync(ConvertApplicationInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var organizationId = orgContext.OrganizationId.Value;

        var application = await applicationRepo.GetTrackedByIdAsync(input.ApplicationId, organizationId, ct);
        if (application is null)
            return Result.Fail(ProgramErrors.ApplicationNotFound(input.ApplicationId));

        // Idempotent: already terminal status
        if (application.Status is ApplicationStatus.Closed or ApplicationStatus.Converted)
            return Result.Success();

        Guid personId;
        if (application.PersonId is not null)
        {
            personId = application.PersonId.Value;
        }
        else
        {
            var personExists = await personReader.ExistsInOrganizationAsync(input.PersonId, organizationId, ct);
            if (!personExists)
                return Result.Fail(ProgramErrors.PersonNotInOrganization(input.PersonId));

            personId = input.PersonId;
        }

        var now = clock.UtcNow.UtcDateTime;

        // Assign person to application if not already set
        if (application.PersonId is null)
            application.AssignPerson(personId, now);

        // Check if enrollment already exists
        var existingEnrollmentId = await enrollmentRepo.GetIdAsync(application.ProgramId, personId, organizationId, ct);

        Guid enrollmentId;
        if (existingEnrollmentId.HasValue)
        {
            enrollmentId = existingEnrollmentId.Value;
        }
        else
        {
            var enrollment = new Enrollment(organizationId, application.ProgramId, personId, now);
            enrollmentRepo.Add(enrollment);
            enrollmentId = enrollment.Id;
        }

        application.MarkConverted(enrollmentId, now);
        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}

