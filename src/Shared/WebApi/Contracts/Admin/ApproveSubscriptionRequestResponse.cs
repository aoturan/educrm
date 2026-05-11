namespace EduCrm.WebApi.Contracts.Admin;

public sealed record ApproveSubscriptionRequestResponse(
    Guid SubscriptionRequestId,
    DateTime ApprovedAtUtc,
    Guid SubscriptionId,
    string PlanCode,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc);
