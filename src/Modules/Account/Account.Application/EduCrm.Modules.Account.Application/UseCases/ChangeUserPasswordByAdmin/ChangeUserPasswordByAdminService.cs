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
    IClock clock,
    ICurrentUserSnapshot currentUser)
    : IChangeUserPasswordByAdminService
{
    public async Task<Result> ChangeAsync(ChangeUserPasswordByAdminInput input, CancellationToken ct)
    {
        if (currentUser.Role != UserRole.Admin)
            return Result.Fail(AccountErrors.NotAdmin());

        var target = await userRepo.GetByIdAsync(input.TargetUserId, ct);
        if (target is null)
            return Result.Fail(AccountErrors.NotFound(input.TargetUserId));

        if (target.OrganizationId != currentUser.OrganizationId)
            return Result.Fail(AccountErrors.UserNotInOrganization());

        if (target.Status != UserStatus.Active)
            return Result.Fail(AccountErrors.UserInactive());

        target.ChangePasswordHash(input.NewPasswordHash, clock.UtcNow.UtcDateTime);

        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}
