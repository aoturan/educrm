using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ConvertApplication;

public sealed record ConvertApplicationInput(Guid ApplicationId, Guid PersonId);

public interface IConvertApplicationService
{
    Task<Result> ConvertAsync(ConvertApplicationInput input, CancellationToken ct);
}

