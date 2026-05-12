using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.MarkSupportRequestHandled;

public interface IMarkSupportRequestHandledService
{
    Task<Result<MarkSupportRequestHandledResult>> MarkAsync(
        MarkSupportRequestHandledInput input,
        CancellationToken ct);
}
