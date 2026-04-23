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
}