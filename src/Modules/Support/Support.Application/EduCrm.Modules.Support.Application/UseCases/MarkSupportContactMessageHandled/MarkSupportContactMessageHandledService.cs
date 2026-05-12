using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.MarkSupportContactMessageHandled;

public sealed class MarkSupportContactMessageHandledService(
    ISupportContactMessageRepository supportContactMessageRepo,
    IUnitOfWork uow,
    IClock clock)
    : IMarkSupportContactMessageHandledService
{
    public async Task<Result<MarkSupportContactMessageHandledResult>> MarkAsync(
        MarkSupportContactMessageHandledInput input,
        CancellationToken ct)
    {
        var message = await supportContactMessageRepo.GetTrackedByIdAsync(input.SupportContactMessageId, ct);
        if (message is null)
            return Result<MarkSupportContactMessageHandledResult>.Fail(
                CommonErrors.NotFound("SupportContactMessage", input.SupportContactMessageId));

        var now = clock.UtcNow.UtcDateTime;
        message.MarkReviewed(now);

        await uow.SaveChangesAsync(ct);

        return Result<MarkSupportContactMessageHandledResult>.Success(
            new MarkSupportContactMessageHandledResult(
                message.Id,
                message.Status.ToString(),
                message.ReviewedAt!.Value));
    }
}
