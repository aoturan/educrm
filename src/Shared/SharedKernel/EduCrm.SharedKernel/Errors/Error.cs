namespace EduCrm.SharedKernel.Errors;

public sealed record Error(
    string Code,
    string Message,
    IReadOnlyDictionary<string, object>? Metadata = null
);