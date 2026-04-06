using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.SnoozeFollowUp;

public sealed class SnoozeFollowUpService(
    IFollowUpRepository followUpRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : ISnoozeFollowUpService
{
    public async Task<Result<SnoozeFollowUpResult>> SnoozeAsync(SnoozeFollowUpInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<SnoozeFollowUpResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<SnoozeFollowUpResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var followUp = await followUpRepo.GetTrackedByIdAsync(input.FollowUpId, orgContext.OrganizationId.Value, ct);
        if (followUp is null)
            return Result<SnoozeFollowUpResult>.Fail(PeopleErrors.FollowUpNotFound(input.FollowUpId));

        if (followUp.Status != FollowUpStatus.Open && followUp.Status != FollowUpStatus.Snoozed)
            return Result<SnoozeFollowUpResult>.Fail(
                PeopleErrors.FollowUpCannotBeSnoozed(input.FollowUpId, followUp.Status.ToString()));

        if (input.SnoozeUntilUtc <= followUp.DueAtUtc)
            return Result<SnoozeFollowUpResult>.Fail(
                PeopleErrors.SnoozeUntilUtcMustBeAfterDueDate(followUp.DueAtUtc));

        followUp.Snooze(input.SnoozeUntilUtc, clock.UtcNow.UtcDateTime);

        await uow.SaveChangesAsync(ct);

        return Result<SnoozeFollowUpResult>.Success(
            new SnoozeFollowUpResult(followUp.Id, followUp.SnoozedUntilUtc!.Value));
    }
}

