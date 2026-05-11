using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.MarkSubscriptionRequestInvoiced;

public sealed class MarkSubscriptionRequestInvoicedService(
    ISubscriptionRequestRepository subscriptionRequestRepo,
    IUnitOfWork uow,
    IClock clock)
    : IMarkSubscriptionRequestInvoicedService
{
    public async Task<Result<MarkSubscriptionRequestInvoicedResult>> MarkAsync(
        MarkSubscriptionRequestInvoicedInput input,
        CancellationToken ct)
    {
        var request = await subscriptionRequestRepo.GetTrackedByIdAsync(input.SubscriptionRequestId, ct);
        if (request is null)
            return Result<MarkSubscriptionRequestInvoicedResult>.Fail(
                AccountErrors.SubscriptionRequestNotFound(input.SubscriptionRequestId));

        if (request.Status != RequestStatus.Approved)
            return Result<MarkSubscriptionRequestInvoicedResult>.Fail(
                AccountErrors.SubscriptionRequestNotApproved(request.Status.ToString()));

        var now = clock.UtcNow.UtcDateTime;
        request.MarkInvoiced(now);

        await uow.SaveChangesAsync(ct);

        return Result<MarkSubscriptionRequestInvoicedResult>.Success(
            new MarkSubscriptionRequestInvoicedResult(request.Id, request.IsInvoiced, request.UpdatedAtUtc));
    }
}
