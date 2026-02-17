namespace EduCrm.SharedKernel.Errors;

/// <summary>
/// Account domain-specific error factory methods.
/// Centralizes error creation for the Account domain.
/// </summary>
public static class AccountErrors
{
    public static Error EmailTaken(string email) =>
        new(
            Code: ErrorCodes.AccountEmailTaken,
            Message: "Bu e-posta adresi zaten kullanımda.",
            Metadata: new Dictionary<string, object>
            {
                ["email"] = email
            }
        );

    public static Error InvalidCredentials() =>
        new(
            Code: ErrorCodes.AccountInvalidCredentials,
            Message: "Yanlış e-posta veya şifre."
        );

    public static Error InvalidOldPassword() =>
        new(
            Code: ErrorCodes.AccountInvalidOldPassword,
            Message: "Mevcut şifre yanlış."
        );

    public static Error NotFound(Guid userId) =>
        new(
            Code: ErrorCodes.AccountNotFound,
            Message: "User account was not found.",
            Metadata: new Dictionary<string, object>
            {
                ["userId"] = userId
            }
        );

    public static Error InvalidToken() =>
        new(
            Code: ErrorCodes.AccountInvalidToken,
            Message: "The provided token is invalid or expired."
        );

    public static Error UserInactive() =>
        new(
            Code: ErrorCodes.AccountUserInactive,
            Message: "User account is not active."
        );
}

