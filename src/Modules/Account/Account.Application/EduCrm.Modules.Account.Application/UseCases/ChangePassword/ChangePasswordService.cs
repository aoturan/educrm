using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ChangePassword;

public sealed class ChangePasswordService(
    IUserRepository userRepo,
    IPasswordHasher passwordHasher,
    IUnitOfWork uow,
    IClock clock,
    ICurrentUserSnapshot currentUser) : IChangePasswordService
{
    public async Task<Result> ChangePasswordAsync(ChangePasswordInput input, CancellationToken ct)
    {
        var user = await userRepo.GetByIdAsync(currentUser.UserId, ct);
        if (user is null)
        {
            return Result.Fail(AccountErrors.NotFound(currentUser.UserId));
        }

        var isOldPasswordValid = passwordHasher.Verify(input.OldPassword, user.PasswordHash);
        if (!isOldPasswordValid)
        {
            return Result.Fail(AccountErrors.InvalidOldPassword());
        }

        user.ChangePasswordHash(input.NewPasswordHash, clock.UtcNow.UtcDateTime);

        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}
