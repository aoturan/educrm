namespace EduCrm.Modules.Program.Application.Errors;

public static class ProgramErrorCodes
{
    public const string SubscriptionRequired = "program.subscription_required";
    public const string SubscriptionInactive = "program.subscription_inactive";
    public const string SubscriptionExpired = "program.subscription_expired";
    public const string SubscriptionInvalid = "program.subscription_invalid";
    public const string OrganizationNotFound = "program.organization_not_found";
    public const string InvalidModalityConfiguration = "program.invalid_modality_configuration";
    public const string EnrollmentNotFound = "program.enrollment_not_found";
    public const string ProgramNotFound = "program.program_not_found";
    public const string PersonNotFound = "program.person_not_found";
    public const string AlreadyEnrolled = "program.already_enrolled";
    public const string ProgramNotActive = "program.program_not_active";
    public const string ProgramEnded = "program.program_ended";
    public const string InvalidStatusTransition = "program.invalid_status_transition";
    public const string UnsupportedTargetStatus = "program.unsupported_target_status";
    public const string EnrollmentDeleteNotAllowed = "program.enrollment_delete_not_allowed";
    public const string ProgramAlreadyArchived = "program.already_archived";
    public const string ProgramNotArchived = "program.not_archived";
    public const string ProgramAlreadyPublic = "program.already_public";
    public const string ProgramNotPublic = "program.not_public";
}