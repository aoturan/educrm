using EduCrm.SharedKernel.Errors;

namespace EduCrm.Modules.Program.Application.Errors;

/// <summary>
/// Provides HTTP status code mappings for Program domain error codes.
/// Registered at application startup via ErrorHttpStatusMapper.Register().
/// </summary>
public static class ProgramErrorMappings
{
    public static IReadOnlyDictionary<string, int> Mappings { get; } = new Dictionary<string, int>
    {
        { ProgramErrorCodes.SubscriptionRequired, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.SubscriptionInactive, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.SubscriptionExpired, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.SubscriptionInvalid, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.OrganizationNotFound, ErrorHttpStatusMapper.Status404NotFound },
        { ProgramErrorCodes.InvalidModalityConfiguration, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.EnrollmentNotFound, ErrorHttpStatusMapper.Status404NotFound },
        { ProgramErrorCodes.ProgramNotFound, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.PersonNotFound, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.AlreadyEnrolled, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.ProgramNotActive, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.ProgramEnded, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.InvalidStatusTransition, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.UnsupportedTargetStatus, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.EnrollmentDeleteNotAllowed, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.ProgramAlreadyArchived, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.ProgramNotArchived, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.ProgramAlreadyPublic, ErrorHttpStatusMapper.Status400BadRequest },
        { ProgramErrorCodes.ProgramNotPublic, ErrorHttpStatusMapper.Status400BadRequest }
    };
}

