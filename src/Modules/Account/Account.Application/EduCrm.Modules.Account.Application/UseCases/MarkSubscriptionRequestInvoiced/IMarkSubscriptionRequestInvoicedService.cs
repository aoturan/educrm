using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.MarkSubscriptionRequestInvoiced;

public interface IMarkSubscriptionRequestInvoicedService
{
    Task<Result<MarkSubscriptionRequestInvoicedResult>> MarkAsync(
        MarkSubscriptionRequestInvoicedInput input,
        CancellationToken ct);
}
