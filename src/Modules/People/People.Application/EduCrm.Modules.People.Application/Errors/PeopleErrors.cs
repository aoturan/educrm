using EduCrm.SharedKernel.Errors;

namespace EduCrm.Modules.People.Application.Errors;

public static class PeopleErrors
{
    public static Error PersonNotFound(Guid personId) =>
        new(
            Code: PeopleErrorCodes.PersonNotFound,
            Message: "Kişi bulunamadı.",
            Metadata: new Dictionary<string, object> { ["personId"] = personId }
        );

    public static Error ProgramNotFound(Guid programId) =>
        new(
            Code: PeopleErrorCodes.ProgramNotFound,
            Message: "Program bulunamadı.",
            Metadata: new Dictionary<string, object> { ["programId"] = programId }
        );

    public static Error FollowUpNotFound(Guid followUpId) =>
        new(
            Code: PeopleErrorCodes.FollowUpNotFound,
            Message: "Takip kaydı bulunamadı.",
            Metadata: new Dictionary<string, object> { ["followUpId"] = followUpId }
        );

    public static Error FollowUpCannotBeUpdated(string status) =>
        new(
            Code: PeopleErrorCodes.FollowUpCannotBeUpdated,
            Message: $"Takip kaydı '{status}' durumunda olduğu için güncellenemez."
        );

    public static Error FollowUpCannotBeCompleted(Guid followUpId, string currentStatus) =>
        new(
            Code: PeopleErrorCodes.FollowUpCannotBeCompleted,
            Message: $"Takip kaydı '{currentStatus}' durumunda olduğu için tamamlanamaz.",
            Metadata: new Dictionary<string, object> { ["followUpId"] = followUpId }
        );

    public static Error FollowUpCannotBeCancelled(Guid followUpId, string currentStatus) =>
        new(
            Code: PeopleErrorCodes.FollowUpCannotBeCancelled,
            Message: $"Takip kaydı '{currentStatus}' durumunda olduğu için iptal edilemez.",
            Metadata: new Dictionary<string, object> { ["followUpId"] = followUpId }
        );

    public static Error FollowUpCannotBeSnoozed(Guid followUpId, string currentStatus) =>
        new(
            Code: PeopleErrorCodes.FollowUpCannotBeSnoozed,
            Message: $"Takip kaydı '{currentStatus}' durumunda olduğu için ertelenemez.",
            Metadata: new Dictionary<string, object> { ["followUpId"] = followUpId }
        );

    public static Error SnoozeUntilUtcMustBeAfterDueDate(DateTime dueAtUtc) =>
        new(
            Code: PeopleErrorCodes.SnoozeUntilUtcMustBeAfterDueDate,
            Message: $"Erteleme tarihi, takip kaydının bitiş tarihinden ({dueAtUtc:O}) sonra olmalıdır.",
            Metadata: new Dictionary<string, object> { ["dueAtUtc"] = dueAtUtc }
        );

    public static Error FollowUpCannotBeRescheduled(Guid followUpId, string currentStatus) =>
        new(
            Code: PeopleErrorCodes.FollowUpCannotBeRescheduled,
            Message: $"Takip kaydı '{currentStatus}' durumunda olduğu için yeniden planlanamaz.",
            Metadata: new Dictionary<string, object> { ["followUpId"] = followUpId }
        );

    public static Error PersonAlreadyArchived(Guid personId) =>
        new(
            Code: PeopleErrorCodes.PersonAlreadyArchived,
            Message: "Kişi zaten arşivlenmiş.",
            Metadata: new Dictionary<string, object> { ["personId"] = personId }
        );

    public static Error PersonNotArchived(Guid personId) =>
        new(
            Code: PeopleErrorCodes.PersonNotArchived,
            Message: "Kişi arşivlenmemiş.",
            Metadata: new Dictionary<string, object> { ["personId"] = personId }
        );

    public static Error PersonCannotBeUpdated(Guid personId) =>
        new(
            Code: PeopleErrorCodes.PersonCannotBeUpdated,
            Message: "Arşivlenmiş kişi güncellenemez.",
            Metadata: new Dictionary<string, object> { ["personId"] = personId }
        );
}
