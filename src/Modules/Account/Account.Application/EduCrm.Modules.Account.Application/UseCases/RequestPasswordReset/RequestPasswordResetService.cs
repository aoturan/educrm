using System.Security.Cryptography;
using System.Text;
using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Email;
using EduCrm.Modules.Account.Application.PasswordReset;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;
using Microsoft.Extensions.Options;

namespace EduCrm.Modules.Account.Application.UseCases.RequestPasswordReset;

public sealed class RequestPasswordResetService(
    IUserRepository userRepo,
    IEmailSender emailSender,
    IUnitOfWork uow,
    IClock clock,
    IOptions<PasswordResetOptions> options) : IRequestPasswordResetService
{
    public async Task<Result> RequestAsync(RequestPasswordResetInput input, CancellationToken ct)
    {
        // Always return Success to prevent account enumeration.
        if (string.IsNullOrWhiteSpace(input.Email))
            return Result.Success();

        var email = input.Email.Trim();
        var user = await userRepo.GetByEmailAsync(email, ct);

        if (user is null || user.Status != UserStatus.Active)
            return Result.Success();

        var opts = options.Value;
        var now = clock.UtcNow.UtcDateTime;

        if (user.PasswordResetRequestedAt is { } lastRequestedAt &&
            now - lastRequestedAt < TimeSpan.FromSeconds(opts.RequestCooldownSeconds))
        {
            return Result.Success();
        }

        var plainToken = GenerateToken();
        var tokenHash = HashToken(plainToken);
        var expiresAt = now.AddMinutes(opts.TokenLifetimeMinutes);

        user.RequestPasswordReset(tokenHash, expiresAt, now);
        await uow.SaveChangesAsync(ct);

        var link = BuildResetLink(opts.ResetUrl, user.Email, plainToken);
        var message = BuildEmail(user.Email, user.FullName, link, opts.TokenLifetimeMinutes);

        try
        {
            await emailSender.SendAsync(message, ct);
        }
        catch
        {
            // Swallow: never reveal delivery failure on this endpoint.
        }

        return Result.Success();
    }

    private static string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static string HashToken(string token)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static string BuildResetLink(string baseUrl, string email, string token)
    {
        var separator = baseUrl.Contains('?') ? '&' : '?';
        return $"{baseUrl}{separator}email={Uri.EscapeDataString(email)}&token={token}";
    }

    private static EmailMessage BuildEmail(string to, string fullName, string link, int lifetimeMinutes)
    {
        var encodedLink = System.Net.WebUtility.HtmlEncode(link);

        var html =
            $"""
             <p>Merhaba {System.Net.WebUtility.HtmlEncode(fullName)},</p>
             <p>Şifre sıfırlama talebinde bulundunuz. Aşağıdaki bağlantıyı kullanarak yeni şifrenizi belirleyebilirsiniz:</p>
             <p><a href="{encodedLink}">{encodedLink}</a></p>
             <p>Bağlantı {lifetimeMinutes} dakika boyunca geçerlidir. Bu talebi siz yapmadıysanız bu e-postayı yok sayabilirsiniz.</p>
             <p>EduCRM</p>
             """;

        var text =
            $"""
             Merhaba {fullName},

             Şifre sıfırlama talebinde bulundunuz. Aşağıdaki bağlantıyı kullanarak yeni şifrenizi belirleyebilirsiniz:

             {link}

             Bağlantı {lifetimeMinutes} dakika boyunca geçerlidir. Bu talebi siz yapmadıysanız bu e-postayı yok sayabilirsiniz.

             EduCRM
             """;

        return new EmailMessage(
            To: to,
            Subject: "Şifre Sıfırlama Talebiniz",
            HtmlBody: html,
            TextBody: text);
    }
}