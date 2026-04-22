using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.FindPersonsForApplication;

public sealed class FindPersonsForApplicationService(
    IApplicationRepository applicationRepo,
    IPersonReader personReader,
    IPersonWriter personWriter,
    IUnitOfWork uow,
    IOrgContext orgContext,
    IClock clock) : IFindPersonsForApplicationService
{
    public async Task<Result<FindPersonsForApplicationResult>> ResolveAsync(
        FindPersonsForApplicationInput input,
        CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<FindPersonsForApplicationResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var organizationId = orgContext.OrganizationId.Value;

        var application = await applicationRepo.GetTrackedByIdAsync(input.ApplicationId, organizationId, ct);
        if (application is null)
            return Result<FindPersonsForApplicationResult>.Fail(ProgramErrors.ApplicationNotFound(input.ApplicationId));

        // If application already has a person assigned, return that person directly
        if (application.PersonId is not null)
        {
            var assigned = await personReader.GetByIdAsync(application.PersonId.Value, organizationId, ct);
            if (assigned is null)
                return Result<FindPersonsForApplicationResult>.Fail(ProgramErrors.PersonNotFound(application.PersonId.Value));

            return Result<FindPersonsForApplicationResult>.Success(
                new FindPersonsForApplicationResult([new PersonMatchData(assigned.PersonId, assigned.FullName, assigned.Email, assigned.Phone)]));
        }

        var persons = await personReader.FindByContactAsync(
            application.SubmittedEmail,
            application.SubmittedPhone,
            organizationId,
            ct);

        if (persons.Count > 0)
        {
            var matches = persons
                .Select(p => new PersonMatchData(p.PersonId, p.FullName, p.Email, p.Phone))
                .ToList();

            return Result<FindPersonsForApplicationResult>.Success(new FindPersonsForApplicationResult(matches));
        }

        // No person found — create one from application data
        var personId = personWriter.AddFromApplication(
            organizationId,
            application.SubmittedFullName,
            application.SubmittedPhone,
            application.SubmittedEmail,
            clock.UtcNow.UtcDateTime);

        await uow.SaveChangesAsync(ct);

        var created = new PersonMatchData(personId, application.SubmittedFullName, application.SubmittedEmail, application.SubmittedPhone);

        return Result<FindPersonsForApplicationResult>.Success(new FindPersonsForApplicationResult([created]));
    }
}
