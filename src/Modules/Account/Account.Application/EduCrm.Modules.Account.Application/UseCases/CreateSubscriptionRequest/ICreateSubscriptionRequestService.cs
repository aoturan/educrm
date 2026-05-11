using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.CreateSubscriptionRequest;

public interface ICreateSubscriptionRequestService
{
    Task<Result<CreateSubscriptionRequestResult>> CreateAsync(
        CreateSubscriptionRequestInput input,
        CancellationToken ct);
}