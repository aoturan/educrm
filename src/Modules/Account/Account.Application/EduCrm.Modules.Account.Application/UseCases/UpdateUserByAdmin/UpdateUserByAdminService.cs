using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpdateUserByAdmin;

public sealed class UpdateUserByAdminService(
    IUserRepository userRepo,
    IUnitOfWork uow,
    IClock clock)
    : IUpdateUserByAdminService
{
    public async Task<Result<UpdateUserByAdminResult>> UpdateAsync(UpdateUserByAdminInput input, CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<UpdateUserByAdminResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<UpdateUserByAdminResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Status != UserStatus.Active)
            return Result<UpdateUserByAdminResult>.Fail(AccountErrors.UserInactive());

        if (caller.Role != UserRole.Admin)
            return Result<UpdateUserByAdminResult>.Fail(AccountErrors.NotAdmin());

        var target = await userRepo.GetByIdAsync(input.TargetUserId, ct);
        if (target is null)
            return Result<UpdateUserByAdminResult>.Fail(AccountErrors.NotFound(input.TargetUserId));

        if (target.OrganizationId != caller.OrganizationId)
            return Result<UpdateUserByAdminResult>.Fail(AccountErrors.UserNotInOrganization());

        if (target.Status != UserStatus.Active)
            return Result<UpdateUserByAdminResult>.Fail(AccountErrors.UserInactive());

        var now = clock.UtcNow.UtcDateTime;
        target.ChangeFullName(input.FullName, now);

        await uow.SaveChangesAsync(ct);

        return Result<UpdateUserByAdminResult>.Success(new UpdateUserByAdminResult(
            target.Id,
            target.Email,
            target.FullName,
            target.Role,
            target.Status,
            target.LastLoginAtUtc));
    }
}
