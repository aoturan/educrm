using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpsertBillingDetail;

public interface IUpsertBillingDetailService
{
    Task<Result<UpsertBillingDetailResult>> UpsertAsync(UpsertBillingDetailInput input, CancellationToken ct);
}