using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.AssignPersonToApplication;

public sealed class AssignPersonToApplicationService(
    IApplicationRepository applicationRepo,
    IPersonReader personReader,
    IPersonWriter personWriter,
    IUnitOfWork uow,
    IAppDbTransaction tx,
    IOrgContext orgContext,
    IClock clock) : IAssignPersonToApplicationService
{
    public async Task<Result<AssignPersonResult>> AssignAsync(AssignPersonInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<AssignPersonResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var organizationId = orgContext.OrganizationId.Value;

        var application = await applicationRepo.GetTrackedByIdAsync(input.ApplicationId, organizationId, ct);
        if (application is null)
            return Result<AssignPersonResult>.Fail(ProgramErrors.ApplicationNotFound(input.ApplicationId));

        var persons = await personReader.FindByContactAsync(
            application.SubmittedEmail,
            application.SubmittedPhone,
            organizationId,
            ct);

        if (persons.Count > 1)
        {
            var candidates = persons
                .Select(p => new PersonCandidateData(p.PersonId, p.FullName, p.Email, p.Phone))
                .ToList();

            return Result<AssignPersonResult>.Success(new AssignPersonResult(IsAmbiguous: true, candidates));
        }

        await using var trx = await tx.BeginAsync(ct);

        try
        {
            Guid personId;

            if (persons.Count == 1)
            {
                personId = persons[0].PersonId;
            }
            else
            {
                var fullName = string.IsNullOrWhiteSpace(application.SubmittedFullName)
                    ? application.SubmittedEmail
                    : application.SubmittedFullName.Trim();

                personId = personWriter.AddFromApplication(
                    organizationId,
                    fullName,
                    application.SubmittedPhone,
                    application.SubmittedEmail,
                    clock.UtcNow.UtcDateTime);
            }

            application.AssignPerson(personId, clock.UtcNow.UtcDateTime);
            await uow.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);

            return Result<AssignPersonResult>.Success(new AssignPersonResult(IsAmbiguous: false, []));
        }
        catch
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }
}
