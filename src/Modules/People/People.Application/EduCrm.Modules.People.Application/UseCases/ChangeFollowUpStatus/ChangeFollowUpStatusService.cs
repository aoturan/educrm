using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ChangeFollowUpStatus;

public sealed class ChangeFollowUpStatusService(
    IFollowUpRepository followUpRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IChangeFollowUpStatusService
{
    public async Task<Result<ChangeFollowUpStatusResult>> ChangeAsync(
        ChangeFollowUpStatusInput input,
        CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<ChangeFollowUpStatusResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<ChangeFollowUpStatusResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var followUp = await followUpRepo.GetTrackedByIdAsync(input.FollowUpId, orgContext.OrganizationId.Value, ct);
        if (followUp is null)
            return Result<ChangeFollowUpStatusResult>.Fail(PeopleErrors.FollowUpNotFound(input.FollowUpId));

        if (followUp.Status != FollowUpStatus.Open && followUp.Status != FollowUpStatus.Snoozed)
        {
            return input.TargetStatus == FollowUpStatus.Completed
                ? Result<ChangeFollowUpStatusResult>.Fail(
                    PeopleErrors.FollowUpCannotBeCompleted(input.FollowUpId, followUp.Status.ToString()))
                : Result<ChangeFollowUpStatusResult>.Fail(
                    PeopleErrors.FollowUpCannotBeCancelled(input.FollowUpId, followUp.Status.ToString()));
        }

        var now = clock.UtcNow.UtcDateTime;

        if (input.TargetStatus == FollowUpStatus.Completed)
            followUp.Complete(now);
        else
            followUp.Cancel(now);

        await uow.SaveChangesAsync(ct);

        return Result<ChangeFollowUpStatusResult>.Success(new ChangeFollowUpStatusResult(
            followUp.Id,
            followUp.Status,
            followUp.CompletedAtUtc,
            followUp.CancelledAtUtc));
    }
}

