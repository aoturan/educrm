using EduCrm.SharedKernel.Errors;

namespace EduCrm.Modules.Account.Application.Errors;

/// <summary>
/// Provides HTTP status code mappings for Account domain error codes.
/// Registered at application startup via ErrorHttpStatusMapper.Register().
/// </summary>
public static class AccountErrorMappings
{
    public static IReadOnlyDictionary<string, int> Mappings { get; } = new Dictionary<string, int>
    {
        { AccountErrorCodes.EmailTaken, ErrorHttpStatusMapper.Status409Conflict },
        { AccountErrorCodes.InvalidCredentials, ErrorHttpStatusMapper.Status401Unauthorized },
        { AccountErrorCodes.InvalidOldPassword, ErrorHttpStatusMapper.Status400BadRequest },
        { AccountErrorCodes.NotFound, ErrorHttpStatusMapper.Status404NotFound },
        { AccountErrorCodes.InvalidToken, ErrorHttpStatusMapper.Status401Unauthorized },
        { AccountErrorCodes.UserInactive, ErrorHttpStatusMapper.Status403Forbidden }
    };
}
