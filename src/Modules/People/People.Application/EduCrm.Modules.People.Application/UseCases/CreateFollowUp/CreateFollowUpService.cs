using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Contracts.Abstractions;
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
    IPlanLimitsResolver planLimitsResolver,
    IUnitOfWork uow,
    IClock clock,
    ICurrentUserSnapshot user) : ICreateFollowUpService
{
    public async Task<Result<CreateFollowUpResult>> CreateAsync(CreateFollowUpInput input, CancellationToken ct)
    {
        var orgId = user.OrganizationId;

        var person = await personRepo.GetByIdAsync(input.PersonId, orgId, ct);
        if (person is null)
            return Result<CreateFollowUpResult>.Fail(PeopleErrors.PersonNotFound(input.PersonId));

        if (input.ProgramId is not null)
        {
            var program = await programReader.GetProgramByIdAsync(input.ProgramId.Value, orgId, ct);
            if (program is null)
                return Result<CreateFollowUpResult>.Fail(PeopleErrors.ProgramNotFound(input.ProgramId.Value));
        }

        var limits = await planLimitsResolver.ResolveAsync(orgId, ct);
        if (limits.OpenFollowUps is int cap)
        {
            var currentOpen = await followUpRepo.CountOpenByOrganizationAsync(orgId, ct);
            if (currentOpen >= cap)
                return Result<CreateFollowUpResult>.Fail(PeopleErrors.PlanOpenFollowUpLimitReached(cap));
        }

        var now = clock.UtcNow.UtcDateTime;

        var followUp = new FollowUp(
            orgId,
            input.PersonId,
            user.UserId,
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