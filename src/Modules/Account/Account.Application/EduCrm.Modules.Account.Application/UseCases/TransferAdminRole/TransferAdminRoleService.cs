using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.TransferAdminRole;

public sealed class TransferAdminRoleService(
    IUserRepository userRepo,
    IUnitOfWork uow,
    IClock clock,
    ICurrentUserSnapshot currentUser)
    : ITransferAdminRoleService
{
    public async Task<Result> TransferAsync(TransferAdminRoleInput input, CancellationToken ct)
    {
        if (currentUser.Role != UserRole.Admin)
            return Result.Fail(AccountErrors.NotAdmin());

        if (input.TargetUserId == currentUser.UserId)
            return Result.Fail(AccountErrors.CannotTransferRoleToSelf());

        var target = await userRepo.GetByIdAsync(input.TargetUserId, ct);
        if (target is null)
            return Result.Fail(AccountErrors.NotFound(input.TargetUserId));

        if (target.OrganizationId != currentUser.OrganizationId)
            return Result.Fail(AccountErrors.UserNotInOrganization());

        if (target.Status != UserStatus.Active)
            return Result.Fail(AccountErrors.UserInactive());

        if (target.Role == UserRole.Admin)
            return Result.Fail(AccountErrors.UserAlreadyAdmin());

        var caller = await userRepo.GetByIdAsync(currentUser.UserId, ct);
        if (caller is null)
            return Result.Fail(AccountErrors.NotFound(currentUser.UserId));

        var now = clock.UtcNow.UtcDateTime;

        target.ChangeRole(UserRole.Admin, now);
        caller.ChangeRole(UserRole.Member, now);

        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}
