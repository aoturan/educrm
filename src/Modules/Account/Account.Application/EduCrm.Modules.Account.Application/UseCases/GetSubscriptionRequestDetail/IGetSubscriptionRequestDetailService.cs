using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetSubscriptionRequestDetail;

public interface IGetSubscriptionRequestDetailService
{
    Task<Result<GetSubscriptionRequestDetailResult>> GetAsync(GetSubscriptionRequestDetailInput input, CancellationToken ct);
}
