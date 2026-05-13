using EduCrm.SharedKernel.Errors;

namespace EduCrm.Modules.Account.Application.Errors;

/// <summary>
/// Provides HTTP status code mappings for Account domain error codes.
/// Registered at application startup via ErrorHttpStatusMapper.Register().
/// </summary>
public static class AccountErrorMappings
{
    public static IReadOnlyDictionary<string, int> Mappings { get; } = new Dictionary<string, int>
    {
        { AccountErrorCodes.EmailTaken, ErrorHttpStatusMapper.Status409Conflict },
        { AccountErrorCodes.InvalidCredentials, ErrorHttpStatusMapper.Status401Unauthorized },
        { AccountErrorCodes.InvalidOldPassword, ErrorHttpStatusMapper.Status400BadRequest },
        { AccountErrorCodes.NotFound, ErrorHttpStatusMapper.Status404NotFound },
        { AccountErrorCodes.InvalidToken, ErrorHttpStatusMapper.Status401Unauthorized },
        { AccountErrorCodes.UserInactive, ErrorHttpStatusMapper.Status403Forbidden },
        { AccountErrorCodes.InvalidPhoneFormat, ErrorHttpStatusMapper.Status400BadRequest },
        { AccountErrorCodes.UserNotInOrganization, ErrorHttpStatusMapper.Status403Forbidden },
        { AccountErrorCodes.NotAdmin, ErrorHttpStatusMapper.Status403Forbidden },
        { AccountErrorCodes.UserAlreadyInStatus, ErrorHttpStatusMapper.Status409Conflict },
        { AccountErrorCodes.CannotChangeOwnStatus, ErrorHttpStatusMapper.Status403Forbidden },
        { AccountErrorCodes.EmailChangeNotAllowed, ErrorHttpStatusMapper.Status403Forbidden },
        { AccountErrorCodes.CannotTransferRoleToSelf, ErrorHttpStatusMapper.Status403Forbidden },
        { AccountErrorCodes.UserAlreadyAdmin, ErrorHttpStatusMapper.Status409Conflict },
        { AccountErrorCodes.PlanUserLimitReached, ErrorHttpStatusMapper.Status403Forbidden },
        { AccountErrorCodes.BillingDetailsNotConfigured, ErrorHttpStatusMapper.Status404NotFound },
        { AccountErrorCodes.SubscriptionPlanNotEligible, ErrorHttpStatusMapper.Status400BadRequest },
        { AccountErrorCodes.NoActiveSubscriptionRequest, ErrorHttpStatusMapper.Status400BadRequest },
        { AccountErrorCodes.PaymentNotificationAlreadyExists, ErrorHttpStatusMapper.Status400BadRequest },
        { AccountErrorCodes.InvalidOrExpiredPasswordReset, ErrorHttpStatusMapper.Status400BadRequest },
        { AccountErrorCodes.OrganizationNotFound, ErrorHttpStatusMapper.Status404NotFound },
        { AccountErrorCodes.SubscriptionRequestNotFound, ErrorHttpStatusMapper.Status404NotFound },
        { AccountErrorCodes.PaymentNotificationReceiptNotFound, ErrorHttpStatusMapper.Status404NotFound },
        { AccountErrorCodes.SubscriptionRequestAlreadyTerminal, ErrorHttpStatusMapper.Status409Conflict },
        { AccountErrorCodes.SubscriptionNotFound, ErrorHttpStatusMapper.Status404NotFound },
        { AccountErrorCodes.SubscriptionRequestNotApproved, ErrorHttpStatusMapper.Status409Conflict },
        { AccountErrorCodes.InvalidSubscriptionPeriod, ErrorHttpStatusMapper.Status400BadRequest },
        { AccountErrorCodes.InvalidOrExpiredEmailVerification, ErrorHttpStatusMapper.Status400BadRequest },
        { AccountErrorCodes.EmailAlreadyVerified, ErrorHttpStatusMapper.Status409Conflict }
    };
}