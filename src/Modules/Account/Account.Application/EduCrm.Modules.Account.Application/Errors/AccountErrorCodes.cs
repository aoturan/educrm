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
    public const string InvalidPhoneFormat = "account.invalid_phone_format";
    public const string UserNotInOrganization = "account.user_not_in_organization";
    public const string NotAdmin = "account.not_admin";
    public const string UserAlreadyInStatus = "account.user_already_in_status";
    public const string CannotChangeOwnStatus = "account.cannot_change_own_status";
    public const string EmailChangeNotAllowed = "account.email_change_not_allowed";
}