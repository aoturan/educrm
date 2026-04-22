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
        InvalidModalityConfiguration("Online katılım için bilgisi girilmelidir.");

    public static Error OnlineModalityMustNotHaveLocationDetails() =>
        InvalidModalityConfiguration("Online modda lokasyon bilgidsi boş olmalıdır.");

    public static Error OnsiteOrHybridModalityRequiresLocationDetails() =>
        InvalidModalityConfiguration("Lokasyon bilgisi girilmelidir.");

    public static Error OnsiteOrHybridModalityMustNotHaveParticipationInfo() =>
        InvalidModalityConfiguration("Online bilgi alanı boş olmalıdır.");

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

    public static Error ApplicationAlreadyExists() =>
        new(
            Code: ProgramErrorCodes.ApplicationAlreadyExists,
            Message: "Daha önce bu bilgiler ile başvuru yapılmıştır."
        );

    public static Error AmbiguousPersonMatch() =>
        new(
            Code: ProgramErrorCodes.AmbiguousPersonMatch,
            Message: "Girdiğiniz bilgilerle birden fazla kayıt eşleşti. Lütfen akademi ile iletişime geçiniz."
        );

    public static Error ProgramNotAvailable() =>
        new(
            Code: ProgramErrorCodes.ProgramNotAvailable,
            Message: "Program bulunamadı veya başvuruya kapalı."
        );

    public static Error InvalidPhoneFormat() =>
        new(
            Code: ProgramErrorCodes.InvalidPhoneFormat,
            Message: "Telefon numarası geçerli bir formatta değil."
        );

    public static Error ApplicationNotFound(Guid applicationId) =>
        new(
            Code: ProgramErrorCodes.ApplicationNotFound,
            Message: "Başvuru bulunamadı.",
            Metadata: new Dictionary<string, object> { ["applicationId"] = applicationId }
        );

    public static Error ApplicationNotNew() =>
        new(
            Code: ProgramErrorCodes.ApplicationNotNew,
            Message: "Bu başvuru yeni durumunda değil, işlem yapılamaz."
        );

    public static Error ApplicationAlreadyHasPerson() =>
        new(
            Code: ProgramErrorCodes.ApplicationAlreadyHasPerson,
            Message: "Bu başvuruya zaten bir kişi atanmış."
        );

    public static Error PersonNotInOrganization(Guid personId) =>
        new(
            Code: ProgramErrorCodes.PersonNotInOrganization,
            Message: "Kişi bu organizasyona ait değil.",
            Metadata: new Dictionary<string, object> { ["personId"] = personId }
        );

    public static Error ApplicationCannotBeClosed() =>
        new(
            Code: ProgramErrorCodes.ApplicationCannotBeClosed,
            Message: "Bu başvuru kapatılamaz. Yalnızca 'Yeni' veya 'İletişime Geçildi' durumundaki başvurular kapatılabilir."
        );
}