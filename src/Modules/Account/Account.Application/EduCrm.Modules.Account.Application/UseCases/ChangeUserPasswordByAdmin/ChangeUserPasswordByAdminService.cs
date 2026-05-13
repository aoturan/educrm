using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ChangeUserPasswordByAdmin;

public sealed class ChangeUserPasswordByAdminService(
    IUserRepository userRepo,
    IUnitOfWork uow,
    IClock clock)
    : IChangeUserPasswordByAdminService
{
    public async Task<Result> ChangeAsync(ChangeUserPasswordByAdminInput input, CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Role != UserRole.Admin)
            return Result.Fail(AccountErrors.NotAdmin());

        var target = await userRepo.GetByIdAsync(input.TargetUserId, ct);
        if (target is null)
            return Result.Fail(AccountErrors.NotFound(input.TargetUserId));

        if (target.OrganizationId != caller.OrganizationId)
            return Result.Fail(AccountErrors.UserNotInOrganization());

        if (target.Status != UserStatus.Active)
            return Result.Fail(AccountErrors.UserInactive());

        target.ChangePasswordHash(input.NewPasswordHash, clock.UtcNow.UtcDateTime);

        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}
