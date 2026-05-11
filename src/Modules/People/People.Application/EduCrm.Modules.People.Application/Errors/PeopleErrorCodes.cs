namespace EduCrm.Modules.People.Application.Errors;

public static class PeopleErrorCodes
{
    public const string PersonNotFound = "people.person_not_found";
    public const string ProgramNotFound = "people.program_not_found";
    public const string FollowUpNotFound = "people.follow_up_not_found";
    public const string FollowUpCannotBeUpdated = "people.follow_up_cannot_be_updated";
    public const string FollowUpCannotBeCompleted = "people.follow_up_cannot_be_completed";
    public const string FollowUpCannotBeCancelled = "people.follow_up_cannot_be_cancelled";
    public const string FollowUpCannotBeSnoozed = "people.follow_up_cannot_be_snoozed";
    public const string SnoozeUntilUtcMustBeAfterDueDate = "people.snooze_until_utc_must_be_after_due_date";
    public const string FollowUpCannotBeRescheduled = "people.follow_up_cannot_be_rescheduled";
    public const string PersonAlreadyArchived = "people.person_already_archived";
    public const string PersonNotArchived = "people.person_not_archived";
    public const string PersonCannotBeUpdated = "people.person_cannot_be_updated";
    public const string InvalidPhoneFormat = "people.invalid_phone_format";
    public const string DuplicateContactInfo = "people.duplicate_contact_info";
    public const string PhoneRequired = "people.phone_required";
    public const string EmailRequired = "people.email_required";
    public const string ContactInfoRequired = "people.contact_info_required";
    public const string PlanActivePersonLimitReached = "people.plan_active_person_limit_reached";
    public const string PlanOpenFollowUpLimitReached = "people.plan_open_followup_limit_reached";
    public const string ExportNotAllowedOnPlan = "people.export_not_allowed_on_plan";
    public const string ExportRateLimited = "people.export_rate_limited";
    public const string ExportRowLimitExceeded = "people.export_row_limit_exceeded";
}
