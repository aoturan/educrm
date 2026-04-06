namespace EduCrm.Modules.Account.Application.Errors;

/// <summary>
/// Error code constants for the Account domain.
/// </summary>
public static class AccountErrorCodes
{
    public const string EmailTaken = "account.email_taken";
    public const string InvalidCredentials = "account.invalid_credentials";
    public const string InvalidOldPassword = "account.invalid_old_password";
    public const string NotFound = "account.not_found";
    public const string InvalidToken = "account.invalid_token";
    public const string UserInactive = "account.user_inactive";
}

