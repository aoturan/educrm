using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.RescheduleDueDate;

public sealed class RescheduleDueDateService(
    IFollowUpRepository followUpRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IRescheduleDueDateService
{
    public async Task<Result<RescheduleDueDateResult>> RescheduleAsync(
        RescheduleDueDateInput input,
        CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<RescheduleDueDateResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<RescheduleDueDateResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var followUp = await followUpRepo.GetTrackedByIdAsync(input.FollowUpId, orgContext.OrganizationId.Value, ct);
        if (followUp is null)
            return Result<RescheduleDueDateResult>.Fail(PeopleErrors.FollowUpNotFound(input.FollowUpId));

        if (followUp.Status == FollowUpStatus.Completed || followUp.Status == FollowUpStatus.Cancelled)
            return Result<RescheduleDueDateResult>.Fail(
                PeopleErrors.FollowUpCannotBeRescheduled(input.FollowUpId, followUp.Status.ToString()));

        followUp.RescheduleDueDate(input.NewDueAtUtc, clock.UtcNow.UtcDateTime);

        await uow.SaveChangesAsync(ct);

        return Result<RescheduleDueDateResult>.Success(new RescheduleDueDateResult(
            followUp.Id,
            followUp.DueAtUtc,
            followUp.Status,
            followUp.SnoozedUntilUtc));
    }
}

