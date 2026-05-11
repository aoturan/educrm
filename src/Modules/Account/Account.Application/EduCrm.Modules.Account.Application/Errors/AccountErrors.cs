using EduCrm.SharedKernel.Errors;

namespace EduCrm.Modules.Account.Application.Errors;

/// <summary>
/// Account domain-specific error factory methods.
/// </summary>
public static class AccountErrors
{
    public static Error EmailTaken(string email) =>
        new(
            Code: AccountErrorCodes.EmailTaken,
            Message: "Bu e-posta adresi zaten kullanımda.",
            Metadata: new Dictionary<string, object>
            {
                ["email"] = email
            }
        );

    public static Error InvalidCredentials() =>
        new(
            Code: AccountErrorCodes.InvalidCredentials,
            Message: "Yanlış e-posta veya şifre."
        );

    public static Error InvalidOldPassword() =>
        new(
            Code: AccountErrorCodes.InvalidOldPassword,
            Message: "Mevcut şifre yanlış."
        );

    public static Error NotFound(Guid userId) =>
        new(
            Code: AccountErrorCodes.NotFound,
            Message: "Kullanıcı hesabı bulunamadı.",
            Metadata: new Dictionary<string, object>
            {
                ["userId"] = userId
            }
        );

    public static Error InvalidToken() =>
        new(
            Code: AccountErrorCodes.InvalidToken,
            Message: "Sağlanan token geçersiz veya süresi dolmuş."
        );

    public static Error UserInactive() =>
        new(
            Code: AccountErrorCodes.UserInactive,
            Message: "Kullanıcı hesabı aktif değil."
        );

    public static Error InvalidPhoneFormat() =>
        new(
            Code: AccountErrorCodes.InvalidPhoneFormat,
            Message: "Geçersiz telefon numarası formatı."
        );

    public static Error UserNotInOrganization() =>
        new(
            Code: AccountErrorCodes.UserNotInOrganization,
            Message: "Kullanıcı bu organizasyona ait değil."
        );

    public static Error NotAdmin() =>
        new(
            Code: AccountErrorCodes.NotAdmin,
            Message: "Bu işlem için yönetici yetkisi gereklidir."
        );

    public static Error UserAlreadyInStatus(string currentStatus) =>
        new(
            Code: AccountErrorCodes.UserAlreadyInStatus,
            Message: "Kullanıcı zaten bu durumda.",
            Metadata: new Dictionary<string, object>
            {
                ["currentStatus"] = currentStatus
            }
        );

    public static Error CannotChangeOwnStatus() =>
        new(
            Code: AccountErrorCodes.CannotChangeOwnStatus,
            Message: "Kendi hesabının durumunu değiştiremezsin."
        );

    public static Error EmailChangeNotAllowed() =>
        new(
            Code: AccountErrorCodes.EmailChangeNotAllowed,
            Message: "E-posta adresini yalnızca yöneticiler değiştirebilir."
        );

    public static Error CannotTransferRoleToSelf() =>
        new(
            Code: AccountErrorCodes.CannotTransferRoleToSelf,
            Message: "Yönetici rolünü kendine devredemezsin."
        );

    public static Error UserAlreadyAdmin() =>
        new(
            Code: AccountErrorCodes.UserAlreadyAdmin,
            Message: "Kullanıcı zaten yönetici rolüne sahip."
        );

    public static Error PlanUserLimitReached(int limit) =>
        new(
            Code: AccountErrorCodes.PlanUserLimitReached,
            Message: "Kullanıcı limitine ulaşıldı. Paketinizi yükseltmeniz gerekir.",
            Metadata: new Dictionary<string, object>
            {
                ["limit"] = limit
            }
        );

    public static Error BillingDetailsNotConfigured() =>
        new(
            Code: AccountErrorCodes.BillingDetailsNotConfigured,
            Message: "Fatura bilgileri henüz tanımlanmamış."
        );

    public static Error SubscriptionPlanNotEligible() =>
        new(
            Code: AccountErrorCodes.SubscriptionPlanNotEligible,
            Message: "Seçilen plan uygun değil."
        );

    public static Error NoActiveSubscriptionRequest() =>
        new(
            Code: AccountErrorCodes.NoActiveSubscriptionRequest,
            Message: "Bildirim için aktif bir abonelik talebi bulunamadı."
        );

    public static Error PaymentNotificationAlreadyExists() =>
        new(
            Code: AccountErrorCodes.PaymentNotificationAlreadyExists,
            Message: "Bu abonelik talebi için zaten bir ödeme bildirimi mevcut."
        );

    public static Error InvalidOrExpiredPasswordReset() =>
        new(
            Code: AccountErrorCodes.InvalidOrExpiredPasswordReset,
            Message: "Geçersiz ya da süresi dolmuş istek."
        );

    public static Error OrganizationNotFound(Guid organizationId) =>
        new(
            Code: AccountErrorCodes.OrganizationNotFound,
            Message: "Organizasyon bulunamadı.",
            Metadata: new Dictionary<string, object>
            {
                ["organizationId"] = organizationId
            }
        );

    public static Error SubscriptionRequestNotFound(Guid subscriptionRequestId) =>
        new(
            Code: AccountErrorCodes.SubscriptionRequestNotFound,
            Message: "Abonelik talebi bulunamadı.",
            Metadata: new Dictionary<string, object>
            {
                ["subscriptionRequestId"] = subscriptionRequestId
            }
        );

    public static Error PaymentNotificationReceiptNotFound(Guid subscriptionRequestId) =>
        new(
            Code: AccountErrorCodes.PaymentNotificationReceiptNotFound,
            Message: "Bu abonelik talebi için yüklenmiş bir dekont bulunmuyor.",
            Metadata: new Dictionary<string, object>
            {
                ["subscriptionRequestId"] = subscriptionRequestId
            }
        );

    public static Error SubscriptionRequestAlreadyTerminal(string currentStatus) =>
        new(
            Code: AccountErrorCodes.SubscriptionRequestAlreadyTerminal,
            Message: "Abonelik talebi zaten sonlandırılmış (onaylı/reddedilmiş/iptal).",
            Metadata: new Dictionary<string, object>
            {
                ["currentStatus"] = currentStatus
            }
        );

    public static Error SubscriptionNotFound(Guid organizationId) =>
        new(
            Code: AccountErrorCodes.SubscriptionNotFound,
            Message: "Organizasyona ait abonelik kaydı bulunamadı.",
            Metadata: new Dictionary<string, object>
            {
                ["organizationId"] = organizationId
            }
        );

    public static Error SubscriptionRequestNotApproved(string currentStatus) =>
        new(
            Code: AccountErrorCodes.SubscriptionRequestNotApproved,
            Message: "Abonelik talebi onaylanmadığı için fatura işareti konulamaz.",
            Metadata: new Dictionary<string, object>
            {
                ["currentStatus"] = currentStatus
            }
        );

    public static Error InvalidSubscriptionPeriod() =>
        new(
            Code: AccountErrorCodes.InvalidSubscriptionPeriod,
            Message: "Bitiş tarihi başlangıç tarihinden sonra olmalı."
        );
}