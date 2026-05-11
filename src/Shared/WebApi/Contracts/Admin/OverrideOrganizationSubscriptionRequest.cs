using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Admin;

public sealed record OverrideOrganizationSubscriptionRequest(
    PlanCode PlanCode,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc);

public sealed record OverrideOrganizationSubscriptionResponse(
    Guid SubscriptionId,
    Guid OrganizationId,
    string PlanCode,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    DateTime UpdatedAtUtc);
