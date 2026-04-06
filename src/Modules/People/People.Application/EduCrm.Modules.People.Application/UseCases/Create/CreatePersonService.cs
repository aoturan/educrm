using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Application.UseCases.Create;
using EduCrm.Modules.People.Domain.Entities;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.Usecases.Create;

public sealed class CreatePersonService(
    IPersonRepository personRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext) : ICreatePersonService
{
    public async Task<Result<CreateResult>> CreateAsync(CreateInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
        {
            return Result<CreateResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));
        }

        var now = clock.UtcNow.UtcDateTime;

        var person = new Person(
            orgContext.OrganizationId.Value,
            input.FullName,
            input.Source,
            now,
            input.Phone,
            input.Email,
            input.Notes);

        personRepo.Add(person);
        await uow.SaveChangesAsync(ct);

        return Result<CreateResult>.Success(new CreateResult(person.Id));
    }
}
