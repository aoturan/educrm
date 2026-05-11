using System.Security.Cryptography;
using System.Text;
using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ResetPassword;

public sealed class ResetPasswordService(
    IUserRepository userRepo,
    IPasswordHasher passwordHasher,
    IUnitOfWork uow,
    IClock clock) : IResetPasswordService
{
    private const int MinPasswordLength = 8;

    public async Task<Result> ResetAsync(ResetPasswordInput input, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(input.Email) ||
            string.IsNullOrWhiteSpace(input.Token) ||
            string.IsNullOrWhiteSpace(input.NewPassword) ||
            input.NewPassword.Length < MinPasswordLength)
        {
            return Result.Fail(AccountErrors.InvalidOrExpiredPasswordReset());
        }

        var user = await userRepo.GetByEmailAsync(input.Email.Trim(), ct);
        if (user is null || user.Status != UserStatus.Active)
            return Result.Fail(AccountErrors.InvalidOrExpiredPasswordReset());

        if (user.PasswordResetTokenHash is null || user.PasswordResetTokenExpiresAt is null)
            return Result.Fail(AccountErrors.InvalidOrExpiredPasswordReset());

        var now = clock.UtcNow.UtcDateTime;
        if (now >= user.PasswordResetTokenExpiresAt.Value)
            return Result.Fail(AccountErrors.InvalidOrExpiredPasswordReset());

        var providedHash = HashToken(input.Token);
        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.ASCII.GetBytes(providedHash),
                Encoding.ASCII.GetBytes(user.PasswordResetTokenHash)))
        {
            return Result.Fail(AccountErrors.InvalidOrExpiredPasswordReset());
        }

        var newPasswordHash = passwordHasher.Hash(input.NewPassword);
        user.CompletePasswordReset(newPasswordHash, now);
        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static string HashToken(string token)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
