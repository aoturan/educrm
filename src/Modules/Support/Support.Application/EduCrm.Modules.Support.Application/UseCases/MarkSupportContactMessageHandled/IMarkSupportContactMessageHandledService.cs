using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.MarkSupportContactMessageHandled;

public interface IMarkSupportContactMessageHandledService
{
    Task<Result<MarkSupportContactMessageHandledResult>> MarkAsync(
        MarkSupportContactMessageHandledInput input,
        CancellationToken ct);
}
