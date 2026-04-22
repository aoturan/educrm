using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.CloseApplication;

public sealed class CloseApplicationService(
    IApplicationRepository applicationRepo,
    IUnitOfWork uow,
    IOrgContext orgContext,
    IClock clock) : ICloseApplicationService
{
    public async Task<Result> CloseAsync(CloseApplicationInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var organizationId = orgContext.OrganizationId.Value;

        var application = await applicationRepo.GetTrackedByIdAsync(input.ApplicationId, organizationId, ct);
        if (application is null)
            return Result.Fail(ProgramErrors.ApplicationNotFound(input.ApplicationId));

        if (application.Status != ApplicationStatus.New && application.Status != ApplicationStatus.Contacted)
            return Result.Fail(ProgramErrors.ApplicationCannotBeClosed());

        application.Close(input.ClosedNote, clock.UtcNow.UtcDateTime);
        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}

