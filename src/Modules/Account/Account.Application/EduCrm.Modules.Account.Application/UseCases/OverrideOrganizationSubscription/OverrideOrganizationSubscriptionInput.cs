using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.OverrideOrganizationSubscription;

public sealed record OverrideOrganizationSubscriptionInput(
    Guid OrganizationId,
    PlanCode PlanCode,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc);
