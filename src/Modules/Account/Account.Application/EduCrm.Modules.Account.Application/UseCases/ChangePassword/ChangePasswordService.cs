using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ChangePassword;

public sealed class ChangePasswordService(
    IUserRepository userRepo,
    IPasswordHasher passwordHasher,
    IUnitOfWork uow,
    IClock clock) : IChangePasswordService
{
    public async Task<Result> ChangePasswordAsync(ChangePasswordInput input, CancellationToken ct)
    {
        // 1) Get user by ID
        var user = await userRepo.GetByIdAsync(input.UserId, ct);
        if (user is null)
        {
            return Result.Fail(AccountErrors.NotFound(input.UserId));
        }

        // 2) Verify old password
        var isOldPasswordValid = passwordHasher.Verify(input.OldPassword, user.PasswordHash);
        if (!isOldPasswordValid)
        {
            return Result.Fail(AccountErrors.InvalidOldPassword());
        }

        // 3) Update password
        user.ChangePasswordHash(input.NewPasswordHash, clock.UtcNow.UtcDateTime);

        // 4) Save changes
        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}

