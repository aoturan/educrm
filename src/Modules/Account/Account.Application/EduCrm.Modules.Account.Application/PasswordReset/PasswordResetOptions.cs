namespace EduCrm.Modules.Account.Application.PasswordReset;

public sealed class PasswordResetOptions
{
    public int RequestCooldownSeconds { get; init; } = 60;
    public int TokenLifetimeMinutes { get; init; } = 30;
    public string ResetUrl { get; init; } = string.Empty;
}