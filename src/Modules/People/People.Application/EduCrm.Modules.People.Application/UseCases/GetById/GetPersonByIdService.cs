using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.GetById;

public sealed class GetPersonByIdService(
    IPersonRepository personRepo,
    IOrgContext orgContext) : IGetPersonByIdService
{
    public async Task<Result<GetPersonByIdResult>> GetAsync(Guid personId, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<GetPersonByIdResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var person = await personRepo.GetByIdAsync(personId, orgContext.OrganizationId.Value, ct);

        if (person is null)
            return Result<GetPersonByIdResult>.Fail(PeopleErrors.PersonNotFound(personId));

        var enrolledPrograms = await personRepo.GetEnrolledProgramsAsync(personId, orgContext.OrganizationId.Value, ct);
        var followUps = await personRepo.GetFollowUpsAsync(personId, orgContext.OrganizationId.Value, ct);

        return Result<GetPersonByIdResult>.Success(new GetPersonByIdResult(
            person.Id,
            person.FullName,
            person.Email,
            person.Phone,
            person.CreatedAtUtc,
            person.Notes,
            enrolledPrograms,
            followUps,
            person.IsArchived,
            person.ArchivedAtUtc));
    }
}
