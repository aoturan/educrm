namespace EduCrm.WebApi.Contracts.Account;

public sealed record RequestPasswordResetRequest(
    string Email,
    string TurnstileToken);
