using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetBillingDetail;

public interface IGetBillingDetailService
{
    Task<Result<GetBillingDetailResult>> GetAsync(GetBillingDetailInput input, CancellationToken ct);
}