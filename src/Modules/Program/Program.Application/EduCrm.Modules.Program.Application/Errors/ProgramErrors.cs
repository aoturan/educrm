using EduCrm.SharedKernel.Errors;

namespace EduCrm.Modules.Program.Application.Errors;

public static class ProgramErrors
{
    public static Error SubscriptionRequired() =>
        new(
            Code: ProgramErrorCodes.SubscriptionRequired,
            Message: "Program kaydı için abonelik gereklidir."
        );

    public static Error SubscriptionInactive() =>
        new(
            Code: ProgramErrorCodes.SubscriptionInactive,
            Message: "Abonelik aktif değil."
        );

    public static Error SubscriptionExpired() =>
        new(
            Code: ProgramErrorCodes.SubscriptionExpired,
            Message: "Abonelik süresi dolmuş."
        );

    public static Error SubscriptionInvalid() =>
        new(
            Code: ProgramErrorCodes.SubscriptionInvalid,
            Message: "Abonelik geçersiz."
        );

    public static Error OrganizationNotFound(Guid organizationId) =>
        new(
            Code: ProgramErrorCodes.OrganizationNotFound,
            Message: "Organizasyon bulunamadı.",
            Metadata: new Dictionary<string, object>
            {
                ["organizationId"] = organizationId
            }
        );

    public static Error InvalidModalityConfiguration(string message) =>
        new(
            Code: ProgramErrorCodes.InvalidModalityConfiguration,
            Message: message
        );

    public static Error OnlineModalityRequiresParticipationInfo() =>
        InvalidModalityConfiguration("Online modality requires OnlineParticipationInfo to be provided.");

    public static Error OnlineModalityMustNotHaveLocationDetails() =>
        InvalidModalityConfiguration("Online modality must not have LocationDetails.");

    public static Error OnsiteOrHybridModalityRequiresLocationDetails() =>
        InvalidModalityConfiguration("Onsite/Hybrid modality requires LocationDetails to be provided.");

    public static Error OnsiteOrHybridModalityMustNotHaveParticipationInfo() =>
        InvalidModalityConfiguration("Onsite/Hybrid modality must not have OnlineParticipationInfo.");

    public static Error EnrollmentNotFound(Guid enrollmentId) =>
        new(
            Code: ProgramErrorCodes.EnrollmentNotFound,
            Message: "Kayıt bulunamadı.",
            Metadata: new Dictionary<string, object> { ["enrollmentId"] = enrollmentId }
        );

    public static Error ProgramNotFound(Guid programId) =>
        new(
            Code: ProgramErrorCodes.ProgramNotFound,
            Message: "Program bulunamadı.",
            Metadata: new Dictionary<string, object> { ["programId"] = programId }
        );

    public static Error PersonNotFound(Guid personId) =>
        new(
            Code: ProgramErrorCodes.PersonNotFound,
            Message: "Kişi bulunamadı.",
            Metadata: new Dictionary<string, object> { ["personId"] = personId }
        );

    public static Error AlreadyEnrolled() =>
        new(
            Code: ProgramErrorCodes.AlreadyEnrolled,
            Message: "Bu kişi zaten bu programa kayıtlı."
        );

    public static Error ProgramNotActive() =>
        new(
            Code: ProgramErrorCodes.ProgramNotActive,
            Message: "Programa kayıt yapılabilmesi için programın aktif olması gerekir."
        );

    public static Error ProgramEnded() =>
        new(
            Code: ProgramErrorCodes.ProgramEnded,
            Message: "Programın bitiş tarihi geçtiği için kayıt yapılamaz."
        );

    public static Error InvalidStatusTransition(string message) =>
        new(
            Code: ProgramErrorCodes.InvalidStatusTransition,
            Message: message
        );

    public static Error UnsupportedTargetStatus() =>
        new(
            Code: ProgramErrorCodes.UnsupportedTargetStatus,
            Message: "Yalnızca Completed veya Archived durumuna geçiş yapılabilir."
        );

    public static Error EnrollmentDeleteNotAllowed() =>
        new(
            Code: ProgramErrorCodes.EnrollmentDeleteNotAllowed,
            Message: "Eğitim programı tamamlanmış olduğu için öğrenci kaydı silinememiştir."
        );

    public static Error ProgramAlreadyArchived(Guid programId) =>
        new(
            Code: ProgramErrorCodes.ProgramAlreadyArchived,
            Message: "Program zaten arşivlenmiş.",
            Metadata: new Dictionary<string, object> { ["programId"] = programId }
        );

    public static Error ProgramNotArchived(Guid programId) =>
        new(
            Code: ProgramErrorCodes.ProgramNotArchived,
            Message: "Program arşivlenmemiş.",
            Metadata: new Dictionary<string, object> { ["programId"] = programId }
        );

    public static Error ProgramAlreadyPublic(Guid programId) =>
        new(
            Code: ProgramErrorCodes.ProgramAlreadyPublic,
            Message: "Program zaten yayında.",
            Metadata: new Dictionary<string, object> { ["programId"] = programId }
        );

    public static Error ProgramNotPublic(Guid programId) =>
        new(
            Code: ProgramErrorCodes.ProgramNotPublic,
            Message: "Program zaten yayında değil.",
            Metadata: new Dictionary<string, object> { ["programId"] = programId }
        );
}