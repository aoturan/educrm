using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.UpdatePerson;

public sealed class UpdatePersonService(
    IPersonRepository personRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IUpdatePersonService
{
    public async Task<Result<UpdatePersonResult>> UpdateAsync(UpdatePersonInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<UpdatePersonResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<UpdatePersonResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var person = await personRepo.GetTrackedByIdAsync(input.PersonId, orgContext.OrganizationId.Value, ct);
        if (person is null)
            return Result<UpdatePersonResult>.Fail(PeopleErrors.PersonNotFound(input.PersonId));

        if (person.IsArchived)
            return Result<UpdatePersonResult>.Fail(PeopleErrors.PersonCannotBeUpdated(input.PersonId));

        person.Update(input.FullName, input.Phone, input.Email, input.Notes, clock.UtcNow.UtcDateTime);

        await uow.SaveChangesAsync(ct);

        return Result<UpdatePersonResult>.Success(new UpdatePersonResult(person.Id));
    }
}

