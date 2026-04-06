using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ArchivePerson;

public sealed class ArchivePersonService(
    IPersonRepository personRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IArchivePersonService
{
    public async Task<Result<ArchivePersonResult>> ArchiveAsync(ArchivePersonInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<ArchivePersonResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<ArchivePersonResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var person = await personRepo.GetTrackedByIdAsync(input.PersonId, orgContext.OrganizationId.Value, ct);
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

            person.Unarchive(now);
        }

        await uow.SaveChangesAsync(ct);

        return Result<ArchivePersonResult>.Success(new ArchivePersonResult(
            person.Id,
            person.IsArchived,
            person.ArchivedAtUtc));
    }
}

