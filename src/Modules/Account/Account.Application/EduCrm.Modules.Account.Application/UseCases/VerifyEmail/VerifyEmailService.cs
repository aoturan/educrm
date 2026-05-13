using System.Security.Cryptography;
using System.Text;
using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.EmailVerification;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.VerifyEmail;

public sealed class VerifyEmailService(
    IUserRepository userRepo,
    IUnitOfWork uow,
    IClock clock) : IVerifyEmailService
{
    public async Task<Result> VerifyAsync(VerifyEmailInput input, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Token))
            return Result.Fail(AccountErrors.InvalidOrExpiredEmailVerification());

        var user = await userRepo.GetByEmailAsync(input.Email.Trim(), ct);
        if (user is null || user.Status == UserStatus.Disabled)
            return Result.Fail(AccountErrors.InvalidOrExpiredEmailVerification());

        if (user.Status == UserStatus.Active)
            return Result.Fail(AccountErrors.EmailAlreadyVerified());

        if (user.EmailVerificationTokenHash is null || user.EmailVerificationTokenExpiresAt is null)
            return Result.Fail(AccountErrors.InvalidOrExpiredEmailVerification());

        var now = clock.UtcNow.UtcDateTime;
        if (now >= user.EmailVerificationTokenExpiresAt.Value)
            return Result.Fail(AccountErrors.InvalidOrExpiredEmailVerification());

        var providedHash = VerificationTokenGenerator.HashToken(input.Token);
        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.ASCII.GetBytes(providedHash),
                Encoding.ASCII.GetBytes(user.EmailVerificationTokenHash)))
        {
            return Result.Fail(AccountErrors.InvalidOrExpiredEmailVerification());
        }

        user.CompleteEmailVerification(now);
        await uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}
