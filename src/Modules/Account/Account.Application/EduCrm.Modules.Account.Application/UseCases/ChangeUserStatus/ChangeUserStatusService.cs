using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ChangeUserStatus;

public sealed class ChangeUserStatusService(
    IUserRepository userRepo,
    IPlanLimitsResolver planLimitsResolver,
    IUnitOfWork uow,
    IClock clock)
    : IChangeUserStatusService
{
    public async Task<Result> ChangeAsync(ChangeUserStatusInput input, CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Status != UserStatus.Active)
            return Result.Fail(AccountErrors.UserInactive());

        if (caller.Role != UserRole.Admin)
            return Result.Fail(AccountErrors.NotAdmin());

        if (input.TargetUserId == caller.Id)
            return Result.Fail(AccountErrors.CannotChangeOwnStatus());

        if (input.TargetStatus != UserStatus.Active && input.TargetStatus != UserStatus.Disabled)
            return Result.Fail(AccountErrors.UserAlreadyInStatus(input.TargetStatus.ToString()));

        var target = await userRepo.GetByIdAsync(input.TargetUserId, ct);
        if (target is null)
            return Result.Fail(AccountErrors.NotFound(input.TargetUserId));

        if (target.OrganizationId != caller.OrganizationId)
            return Result.Fail(AccountErrors.UserNotInOrganization());

        if (target.Status == input.TargetStatus)
            return Result.Fail(AccountErrors.UserAlreadyInStatus(target.Status.ToString()));

        var now = clock.UtcNow.UtcDateTime;

        if (input.TargetStatus == UserStatus.Active)
        {
            var limits = await planLimitsResolver.ResolveAsync(caller.OrganizationId, ct);
            var currentActive = await userRepo.CountActiveByOrganizationAsync(caller.OrganizationId, ct);
            if (currentActive >= limits.Users)
                return Result.Fail(AccountErrors.PlanUserLimitReached(limits.Users));

            target.Enable(now);
        }
        else
        {
            target.Disable(now);
        }

        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}