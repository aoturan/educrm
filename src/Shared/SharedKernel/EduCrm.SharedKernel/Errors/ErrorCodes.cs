namespace EduCrm.SharedKernel.Errors;

/// <summary>
/// Centralized repository of common error codes shared across all domains.
/// Domain-specific error codes live within each domain's Application layer.
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
    public const string CommonTurnstileFailed = "common.turnstile_failed";
    public const string CommonRateLimited = "common.rate_limited";

    // ===========================
    // Demo/Test Errors
    // ===========================
    public const string DemoFail = "demo.fail";
}
