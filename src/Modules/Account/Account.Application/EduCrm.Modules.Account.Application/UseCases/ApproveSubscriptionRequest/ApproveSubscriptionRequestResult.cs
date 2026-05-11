namespace EduCrm.Modules.Account.Application.UseCases.ApproveSubscriptionRequest;

public sealed record ApproveSubscriptionRequestResult(
    Guid SubscriptionRequestId,
    DateTime ApprovedAtUtc,
    Guid SubscriptionId,
    string PlanCode,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc);
