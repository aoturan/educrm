namespace EduCrm.SharedKernel.Errors;

/// <summary>
/// Centralized repository of all error codes used across the application.
/// Error codes follow the pattern: {domain}.{specific_error}
/// </summary>
public static class ErrorCodes
{
    // ===========================
    // Common Errors
    // ===========================
    public const string CommonValidation = "common.validation";
    public const string CommonUnauthorized = "common.unauthorized";
    public const string CommonForbidden = "common.forbidden";
    public const string CommonNotFound = "common.not_found";
    public const string CommonConflict = "common.conflict";
    
    // ===========================
    // Account Domain Errors
    // ===========================
    public const string AccountEmailTaken = "account.email_taken";
    public const string AccountInvalidCredentials = "account.invalid_credentials";
    public const string AccountInvalidOldPassword = "account.invalid_old_password";
    public const string AccountNotFound = "account.not_found";
    public const string AccountInvalidToken = "account.invalid_token";
    public const string AccountUserInactive = "account.user_inactive";
    
    // ===========================
    // People Domain Errors
    // ===========================
    // Add People domain error codes here as needed
    
    // ===========================
    // Demo/Test Errors
    // ===========================
    public const string DemoFail = "demo.fail";
}

