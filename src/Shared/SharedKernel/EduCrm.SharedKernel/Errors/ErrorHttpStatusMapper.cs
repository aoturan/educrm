using System.Collections.Concurrent;

namespace EduCrm.SharedKernel.Errors;

/// <summary>
/// Maps error codes to HTTP status codes.
/// Each domain registers its own mappings at startup via <see cref="Register"/>.
/// </summary>
public static class ErrorHttpStatusMapper
{
    public const int Status400BadRequest = 400;
    public const int Status401Unauthorized = 401;
    public const int Status403Forbidden = 403;
    public const int Status404NotFound = 404;
    public const int Status409Conflict = 409;

    private static readonly ConcurrentDictionary<string, int> ErrorStatusMap = new()
    {
        // Common errors
        [ErrorCodes.CommonValidation] = Status400BadRequest,
        [ErrorCodes.CommonUnauthorized] = Status401Unauthorized,
        [ErrorCodes.CommonForbidden] = Status403Forbidden,
        [ErrorCodes.CommonNotFound] = Status404NotFound,
        [ErrorCodes.CommonConflict] = Status409Conflict,

        // Demo errors
        [ErrorCodes.DemoFail] = Status400BadRequest
    };

    /// <summary>
    /// Registers error code to HTTP status code mappings.
    /// Intended to be called once per domain at application startup.
    /// </summary>
    public static void Register(IReadOnlyDictionary<string, int> mappings)
    {
        foreach (var (code, status) in mappings)
        {
            ErrorStatusMap[code] = status;
        }
    }

    /// <summary>
    /// Gets the HTTP status code for a given error code.
    /// Returns 400 Bad Request as the default if the error code is not mapped.
    /// </summary>
    public static int GetStatusCode(string errorCode)
    {
        return ErrorStatusMap.TryGetValue(errorCode, out var status)
            ? status
            : Status400BadRequest;
    }
}
