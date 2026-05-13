namespace EduCrm.Modules.Account.Application.EmailVerification;

public sealed class EmailVerificationOptions
{
    public int RequestCooldownSeconds { get; init; } = 60;
    public int TokenLifetimeMinutes { get; init; } = 1440;
    public string VerificationUrl { get; init; } = string.Empty;
}
