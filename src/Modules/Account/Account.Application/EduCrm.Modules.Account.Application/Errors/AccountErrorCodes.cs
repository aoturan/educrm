namespace EduCrm.Modules.Account.Application.Errors;

/// <summary>
/// Error code constants for the Account domain.
/// </summary>
public static class AccountErrorCodes
{
    public const string EmailTaken = "account.email_taken";
    public const string InvalidCredentials = "account.invalid_credentials";
    public const string InvalidOldPassword = "account.invalid_old_password";
    public const string NotFound = "account.not_found";
    public const string InvalidToken = "account.invalid_token";
    public const string UserInactive = "account.user_inactive";
    public const string InvalidPhoneFormat = "account.invalid_phone_format";
    public const string UserNotInOrganization = "account.user_not_in_organization";
    public const string NotAdmin = "account.not_admin";
    public const string UserAlreadyInStatus = "account.user_already_in_status";
    public const string CannotChangeOwnStatus = "account.cannot_change_own_status";
    public const string EmailChangeNotAllowed = "account.email_change_not_allowed";
    public const string CannotTransferRoleToSelf = "account.cannot_transfer_role_to_self";
    public const string UserAlreadyAdmin = "account.user_already_admin";
    public const string PlanUserLimitReached = "account.plan_user_limit_reached";
    public const string BillingDetailsNotConfigured = "account.billing_details_not_configured";
    public const string SubscriptionPlanNotEligible = "account.subscription_plan_not_eligible";
    public const string NoActiveSubscriptionRequest = "account.no_active_subscription_request";
    public const string PaymentNotificationAlreadyExists = "account.payment_notification_already_exists";
    public const string InvalidOrExpiredPasswordReset = "account.invalid_or_expired_password_reset";
    public const string OrganizationNotFound = "account.organization_not_found";
    public const string SubscriptionRequestNotFound = "account.subscription_request_not_found";
    public const string PaymentNotificationReceiptNotFound = "account.payment_notification_receipt_not_found";
    public const string SubscriptionRequestAlreadyTerminal = "account.subscription_request_already_terminal";
    public const string SubscriptionNotFound = "account.subscription_not_found";
    public const string SubscriptionRequestNotApproved = "account.subscription_request_not_approved";
    public const string InvalidSubscriptionPeriod = "account.invalid_subscription_period";
    public const string InvalidOrExpiredEmailVerification = "account.email_verification.invalid_or_expired";
    public const string EmailAlreadyVerified = "account.email_verification.already_verified";
}
