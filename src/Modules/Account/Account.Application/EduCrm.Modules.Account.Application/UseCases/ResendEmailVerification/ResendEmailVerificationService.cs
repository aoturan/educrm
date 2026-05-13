using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Email;
using EduCrm.Modules.Account.Application.EmailVerification;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;
using Microsoft.Extensions.Options;

namespace EduCrm.Modules.Account.Application.UseCases.ResendEmailVerification;

public sealed class ResendEmailVerificationService(
    IUserRepository userRepo,
    IEmailSender emailSender,
    IUnitOfWork uow,
    IClock clock,
    IOptions<EmailVerificationOptions> options) : IResendEmailVerificationService
{
    public async Task<Result> ResendAsync(ResendEmailVerificationInput input, CancellationToken ct)
    {
        // Always return Success to prevent account enumeration.
        if (string.IsNullOrWhiteSpace(input.Email))
            return Result.Success();

        var user = await userRepo.GetByEmailAsync(input.Email.Trim(), ct);
        if (user is null || user.Status != UserStatus.WaitingForActivation)
            return Result.Success();

        var opts = options.Value;
        var now = clock.UtcNow.UtcDateTime;

        if (user.EmailVerificationSentAt is { } lastSentAt &&
            now - lastSentAt < TimeSpan.FromSeconds(opts.RequestCooldownSeconds))
        {
            return Result.Success();
        }

        var plainToken = VerificationTokenGenerator.GenerateToken();
        var tokenHash = VerificationTokenGenerator.HashToken(plainToken);
        var expiresAt = now.AddMinutes(opts.TokenLifetimeMinutes);

        user.IssueEmailVerification(tokenHash, expiresAt, now);
        await uow.SaveChangesAsync(ct);

        try
        {
            var link = EmailVerificationEmailBuilder.BuildLink(opts.VerificationUrl, user.Email, plainToken);
            var message = EmailVerificationEmailBuilder.Build(user.Email, user.FullName, link, opts.TokenLifetimeMinutes);
            await emailSender.SendAsync(message, ct);
        }
        catch
        {
            // Swallow: never reveal delivery failure on this endpoint.
        }

        return Result.Success();
    }
}
