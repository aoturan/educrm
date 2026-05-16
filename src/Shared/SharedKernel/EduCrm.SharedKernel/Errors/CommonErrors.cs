namespace EduCrm.SharedKernel.Errors;

public static class CommonErrors
{
    public static Error NotFound(string entity, object id) =>
        new(
            Code: ErrorCodes.CommonNotFound,
            Message: $"{entity} bulunamadı.",
            Metadata: new Dictionary<string, object>
            {
                ["entity"] = entity,
                ["id"] = id
            }
        );

    public static Error Forbidden(string? reason = null) =>
        new(
            Code: ErrorCodes.CommonForbidden,
            Message: reason is null ? "Erişim reddedildi." : $"Erişim reddedildi: {reason}"
        );

    public static Error Unauthorized() =>
        new(
            Code: ErrorCodes.CommonUnauthorized,
            Message: "Kimlik doğrulaması gereklidir."
        );

    public static Error Conflict(string? reason = null) =>
        new(
            Code: ErrorCodes.CommonConflict,
            Message: reason is null ? "Bir çakışma oluştu." : $"Bir çakışma oluştu: {reason}"
        );

    public static Error Validation(string field, string message) =>
        new(
            Code: ErrorCodes.CommonValidation,
            Message: message,
            Metadata: new Dictionary<string, object>
            {
                ["field"] = field
            }
        );

    public static Error TurnstileFailed(IReadOnlyList<string>? cloudflareErrorCodes = null) =>
        new(
            Code: ErrorCodes.CommonTurnstileFailed,
            Message: "Bot doğrulaması başarısız oldu.",
            Metadata: cloudflareErrorCodes is { Count: > 0 }
                ? new Dictionary<string, object> { ["cloudflareErrors"] = cloudflareErrorCodes }
                : null
        );

    public static Error RateLimited() =>
        new(
            Code: ErrorCodes.CommonRateLimited,
            Message: "Çok fazla istek yaptınız. Lütfen daha sonra tekrar deneyin."
        );
}
