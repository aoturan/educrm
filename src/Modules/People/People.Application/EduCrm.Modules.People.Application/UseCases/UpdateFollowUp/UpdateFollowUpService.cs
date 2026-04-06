using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Domain.Enums;
using EduCrm.Modules.Program.Contracts.Abstractions;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.UpdateFollowUp;

public sealed class UpdateFollowUpService(
    IFollowUpRepository followUpRepo,
    IPersonRepository personRepo,
    IProgramReader programReader,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IUpdateFollowUpService
{
    public async Task<Result<UpdateFollowUpResult>> UpdateAsync(UpdateFollowUpInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<UpdateFollowUpResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<UpdateFollowUpResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var orgId = orgContext.OrganizationId.Value;

        var followUp = await followUpRepo.GetTrackedByIdAsync(input.FollowUpId, orgId, ct);
        if (followUp is null)
            return Result<UpdateFollowUpResult>.Fail(PeopleErrors.FollowUpNotFound(input.FollowUpId));

        if (followUp.Status is FollowUpStatus.Completed or FollowUpStatus.Cancelled)
            return Result<UpdateFollowUpResult>.Fail(PeopleErrors.FollowUpCannotBeUpdated(followUp.Status.ToString()));

        if (followUp.PersonId != input.PersonId)
        {
            var person = await personRepo.GetByIdAsync(input.PersonId, orgId, ct);
            if (person is null)
                return Result<UpdateFollowUpResult>.Fail(PeopleErrors.PersonNotFound(input.PersonId));
        }

        if (input.ProgramId is not null && followUp.ProgramId != input.ProgramId)
        {
            var program = await programReader.GetProgramByIdAsync(input.ProgramId.Value, orgId, ct);
            if (program is null)
                return Result<UpdateFollowUpResult>.Fail(PeopleErrors.ProgramNotFound(input.ProgramId.Value));
        }

        followUp.Update(
            input.PersonId,
            input.ProgramId,
            input.Type,
            input.Title,
            input.Note,
            clock.UtcNow.UtcDateTime);

        await uow.SaveChangesAsync(ct);

        return Result<UpdateFollowUpResult>.Success(new UpdateFollowUpResult(followUp.Id));
    }
}

