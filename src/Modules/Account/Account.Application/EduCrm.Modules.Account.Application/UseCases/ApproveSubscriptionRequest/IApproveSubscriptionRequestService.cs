using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ApproveSubscriptionRequest;

public interface IApproveSubscriptionRequestService
{
    Task<Result<ApproveSubscriptionRequestResult>> ApproveAsync(
        ApproveSubscriptionRequestInput input,
        CancellationToken ct);
}
