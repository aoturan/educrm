using EduCrm.SharedKernel.Results;

namespace EduCrm.Infrastructure.Turnstile;

public interface ITurnstileVerifier
{
    Task<Result> VerifyAsync(string? token, string? remoteIp, CancellationToken ct);
}
