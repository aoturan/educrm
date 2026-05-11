using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.CreateSubscriptionRequest;

public sealed record CreateSubscriptionRequestInput(
    Guid CallerUserId,
    Guid CallerOrganizationId,
    PlanCode RequestedPlanCode);