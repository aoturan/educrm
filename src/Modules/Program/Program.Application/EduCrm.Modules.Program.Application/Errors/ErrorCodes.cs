namespace EduCrm.Modules.Program.Application.Errors;

public static class ProgramErrorCodes
{
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
    public const string ApplicationAlreadyExists = "program.application_already_exists";
    public const string AmbiguousPersonMatch = "program.ambiguous_person_match";
    public const string ProgramNotAvailable = "program.not_available";
    public const string InvalidPhoneFormat = "program.invalid_phone_format";
    public const string ApplicationNotFound = "program.application_not_found";
    public const string ApplicationNotNew = "program.application_not_new";
    public const string ApplicationAlreadyHasPerson = "program.application_already_has_person";
    public const string PersonNotInOrganization = "program.person_not_in_organization";
    public const string ApplicationCannotBeClosed = "program.application_cannot_be_closed";
    public const string PlanActiveProgramLimitReached = "program.plan_active_program_limit_reached";
    public const string PlanActivePersonLimitReached = "program.plan_active_person_limit_reached";
    public const string ExportNotAllowedOnPlan = "program.export_not_allowed_on_plan";
    public const string ExportRateLimited = "program.export_rate_limited";
    public const string ExportRowLimitExceeded = "program.export_row_limit_exceeded";
}