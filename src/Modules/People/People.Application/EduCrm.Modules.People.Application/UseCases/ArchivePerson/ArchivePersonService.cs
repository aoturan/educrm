using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ArchivePerson;

public sealed class ArchivePersonService(
    IPersonRepository personRepo,
    IPlanLimitsResolver planLimitsResolver,
    IUnitOfWork uow,
    IClock clock,
    ICurrentUserSnapshot user) : IArchivePersonService
{
    public async Task<Result<ArchivePersonResult>> ArchiveAsync(ArchivePersonInput input, CancellationToken ct)
    {
        var person = await personRepo.GetTrackedByIdAsync(input.PersonId, user.OrganizationId, ct);
        if (person is null)
            return Result<ArchivePersonResult>.Fail(PeopleErrors.PersonNotFound(input.PersonId));

        var now = clock.UtcNow.UtcDateTime;

        if (input.ShouldArchive)
        {
            if (person.IsArchived)
                return Result<ArchivePersonResult>.Fail(PeopleErrors.PersonAlreadyArchived(input.PersonId));

            person.Archive(now);
        }
        else
        {
            if (!person.IsArchived)
                return Result<ArchivePersonResult>.Fail(PeopleErrors.PersonNotArchived(input.PersonId));

            var limits = await planLimitsResolver.ResolveAsync(user.OrganizationId, ct);
            var currentActive = await personRepo.CountActiveByOrganizationAsync(user.OrganizationId, ct);
            if (currentActive >= limits.ActivePersons)
                return Result<ArchivePersonResult>.Fail(PeopleErrors.PlanActivePersonLimitReached(limits.ActivePersons));

            person.Unarchive(now);
        }

        await uow.SaveChangesAsync(ct);

        return Result<ArchivePersonResult>.Success(new ArchivePersonResult(
            person.Id,
            person.IsArchived,
            person.ArchivedAtUtc));
    }
}

