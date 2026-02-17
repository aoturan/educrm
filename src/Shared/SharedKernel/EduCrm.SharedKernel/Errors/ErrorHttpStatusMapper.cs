namespace EduCrm.SharedKernel.Errors;

/// <summary>
/// Maps error codes to HTTP status codes.
/// This provides a centralized way to determine the appropriate HTTP response status
/// for each error code across all domains.
/// </summary>
public static class ErrorHttpStatusMapper
{
    // HTTP status code constants (to avoid dependency on Microsoft.AspNetCore.Http in SharedKernel)
    private const int Status400BadRequest = 400;
    private const int Status401Unauthorized = 401;
    private const int Status403Forbidden = 403;
    private const int Status404NotFound = 404;
    private const int Status409Conflict = 409;

    private static readonly Dictionary<string, int> ErrorStatusMap = new()
    {
        // Common errors
        { ErrorCodes.CommonValidation, Status400BadRequest },
        { ErrorCodes.CommonUnauthorized, Status401Unauthorized },
        { ErrorCodes.CommonForbidden, Status403Forbidden },
        { ErrorCodes.CommonNotFound, Status404NotFound },
        { ErrorCodes.CommonConflict, Status409Conflict },
        
        // Account domain errors
        { ErrorCodes.AccountEmailTaken, Status409Conflict },
        { ErrorCodes.AccountInvalidCredentials, Status401Unauthorized },
        { ErrorCodes.AccountInvalidOldPassword, Status400BadRequest },
        { ErrorCodes.AccountNotFound, Status404NotFound },
        { ErrorCodes.AccountInvalidToken, Status401Unauthorized },
        { ErrorCodes.AccountUserInactive, Status403Forbidden },
        
        // Demo errors
        { ErrorCodes.DemoFail, Status400BadRequest }
    };

    /// <summary>
    /// Gets the HTTP status code for a given error code.
    /// Returns 400 Bad Request as the default if the error code is not mapped.
    /// </summary>
    public static int GetStatusCode(string errorCode)
    {
        return ErrorStatusMap.TryGetValue(errorCode, out var status)
            ? status
            : Status400BadRequest; // Default fallback
    }
}

