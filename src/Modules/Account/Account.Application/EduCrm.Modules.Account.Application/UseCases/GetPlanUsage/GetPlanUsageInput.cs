namespace EduCrm.Modules.Account.Application.UseCases.GetPlanUsage;

public sealed record GetPlanUsageInput(
    Guid CallerUserId,
    Guid CallerOrganizationId);