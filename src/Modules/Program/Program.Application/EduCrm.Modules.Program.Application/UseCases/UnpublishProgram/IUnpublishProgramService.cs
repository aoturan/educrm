using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.UnpublishProgram;

public interface IUnpublishProgramService
{
    Task<Result<UnpublishProgramResult>> UnpublishAsync(UnpublishProgramInput input, CancellationToken ct);
}

