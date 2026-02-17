using EduCrm.Modules.People.Application.Commands;
using EduCrm.Modules.People.Application.Dtos;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.Services;

public interface IPeopleService
{
    // Intentionally empty for Phase 1 boilerplate.
    // We will add use-cases later (CreatePerson, UpdatePerson, SearchPeople, etc.).
    Task<Result<PersonCreatedDto>> CreatePersonAsync(CreatePersonCommand command, CancellationToken ct);
}