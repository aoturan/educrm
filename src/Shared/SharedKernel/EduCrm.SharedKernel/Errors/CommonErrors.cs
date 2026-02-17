namespace EduCrm.SharedKernel.Errors;

public static class CommonErrors
{
    public static Error NotFound(string entity, object id) =>
        new(
            Code: ErrorCodes.CommonNotFound,
            Message: $"{entity} was not found.",
            Metadata: new Dictionary<string, object>
            {
                ["entity"] = entity,
                ["id"] = id
            }
        );

    public static Error Forbidden(string? reason = null) =>
        new(
            Code: ErrorCodes.CommonForbidden,
            Message: reason is null ? "Access is forbidden." : $"Access is forbidden: {reason}"
        );

    public static Error Unauthorized() =>
        new(
            Code: ErrorCodes.CommonUnauthorized,
            Message: "Authentication is required."
        );

    public static Error Conflict(string? reason = null) =>
        new(
            Code: ErrorCodes.CommonConflict,
            Message: reason is null ? "A conflict occurred." : $"A conflict occurred: {reason}"
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
}
