using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ContactApplication;

public sealed class ContactApplicationService(
    IApplicationRepository applicationRepo,
    IPersonReader personReader,
    IUnitOfWork uow,
    IOrgContext orgContext,
    IClock clock) : IContactApplicationService
{
    public async Task<Result> ContactAsync(ContactApplicationInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var organizationId = orgContext.OrganizationId.Value;

        var application = await applicationRepo.GetTrackedByIdAsync(input.ApplicationId, organizationId, ct);
        if (application is null)
            return Result.Fail(ProgramErrors.ApplicationNotFound(input.ApplicationId));

        if (application.Status != ApplicationStatus.New)
            return Result.Fail(ProgramErrors.ApplicationNotNew());

        if (application.PersonId is not null)
        {
            application.MarkContactedStatus(clock.UtcNow.UtcDateTime);
            await uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        var personExists = await personReader.ExistsInOrganizationAsync(input.PersonId, organizationId, ct);
        if (!personExists)
            return Result.Fail(ProgramErrors.PersonNotInOrganization(input.PersonId));

        application.MarkContacted(input.PersonId, clock.UtcNow.UtcDateTime);
        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}

