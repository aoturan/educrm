using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.MarkSupportRequestHandled;

public sealed class MarkSupportRequestHandledService(
    ISupportRequestRepository supportRequestRepo,
    IUnitOfWork uow,
    IClock clock)
    : IMarkSupportRequestHandledService
{
    public async Task<Result<MarkSupportRequestHandledResult>> MarkAsync(
        MarkSupportRequestHandledInput input,
        CancellationToken ct)
    {
        var request = await supportRequestRepo.GetTrackedByIdAsync(input.SupportRequestId, ct);
        if (request is null)
            return Result<MarkSupportRequestHandledResult>.Fail(
                CommonErrors.NotFound("SupportRequest", input.SupportRequestId));

        var now = clock.UtcNow.UtcDateTime;
        request.MarkHandled(now);

        await uow.SaveChangesAsync(ct);

        return Result<MarkSupportRequestHandledResult>.Success(
            new MarkSupportRequestHandledResult(
                request.Id,
                request.Status.ToString(),
                request.HandledAtUtc!.Value));
    }
}
