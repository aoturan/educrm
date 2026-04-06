using EduCrm.Modules.Account.Contracts.Enums;

namespace EduCrm.Modules.Account.Contracts.Dtos;

public sealed record OrganizationSubscriptionInfo(
    Guid OrganizationId,
    OrganizationPlanType PlanType,
    SubscriptionBillingCycle SubscriptionBillingCycle,
    SubscriptionStatus SubscriptionStatus,
    DateTime? SubscriptionEndsAtUtc,
    DateTime? FreeProgramConsumedAtUtc
);