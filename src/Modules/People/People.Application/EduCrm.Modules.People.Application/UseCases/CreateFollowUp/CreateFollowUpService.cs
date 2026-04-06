using EduCrm.Infrastructure.Persistence;
    using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Domain.Entities;
using EduCrm.Modules.Program.Contracts.Abstractions;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.CreateFollowUp;

public sealed class CreateFollowUpService(
    IFollowUpRepository followUpRepo,
    IPersonRepository personRepo,
    IProgramReader programReader,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : ICreateFollowUpService
{
    public async Task<Result<CreateFollowUpResult>> CreateAsync(CreateFollowUpInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<CreateFollowUpResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<CreateFollowUpResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var orgId = orgContext.OrganizationId.Value;

        var person = await personRepo.GetByIdAsync(input.PersonId, orgId, ct);
        if (person is null)
            return Result<CreateFollowUpResult>.Fail(PeopleErrors.PersonNotFound(input.PersonId));

        if (input.ProgramId is not null)
        {
            var program = await programReader.GetProgramByIdAsync(input.ProgramId.Value, orgId, ct);
            if (program is null)
                return Result<CreateFollowUpResult>.Fail(PeopleErrors.ProgramNotFound(input.ProgramId.Value));
        }

        var now = clock.UtcNow.UtcDateTime;

        var followUp = new FollowUp(
            orgId,
            input.PersonId,
            currentUser.UserId.Value,
            input.Type,
            input.Title,
            input.DueAtUtc,
            now,
            input.ProgramId,
            input.Note);

        followUpRepo.Add(followUp);
        await uow.SaveChangesAsync(ct);

        return Result<CreateFollowUpResult>.Success(new CreateFollowUpResult(followUp.Id));
    }
}
