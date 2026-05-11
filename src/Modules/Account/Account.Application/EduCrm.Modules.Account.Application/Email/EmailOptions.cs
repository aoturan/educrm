namespace EduCrm.Modules.Account.Application.Email;

public sealed class EmailOptions
{
    public string Provider { get; init; } = string.Empty;
    public string FromEmail { get; init; } = string.Empty;
    public string FromName { get; init; } = string.Empty;
    public ResendEmailOptions Resend { get; init; } = new();
}

public sealed class ResendEmailOptions
{
    public string ApiKey { get; init; } = string.Empty;
}