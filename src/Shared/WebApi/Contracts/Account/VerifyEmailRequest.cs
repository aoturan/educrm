namespace EduCrm.WebApi.Contracts.Account;

public sealed record VerifyEmailRequest(string Email, string Token);
