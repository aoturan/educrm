namespace EduCrm.Modules.Account.Application.UseCases.OverrideOrganizationSubscription;

public sealed record OverrideOrganizationSubscriptionResult(
    Guid SubscriptionId,
    Guid OrganizationId,
    string PlanCode,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    DateTime UpdatedAtUtc);
