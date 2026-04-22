using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.CloseApplication;

public sealed record CloseApplicationInput(Guid ApplicationId, string? ClosedNote);

public interface ICloseApplicationService
{
    Task<Result> CloseAsync(CloseApplicationInput input, CancellationToken ct);
}

