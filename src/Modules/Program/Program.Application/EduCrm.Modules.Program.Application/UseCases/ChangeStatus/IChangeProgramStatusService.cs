using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ChangeStatus;

public interface IChangeProgramStatusService
{
    Task<Result<ChangeProgramStatusResult>> ChangeAsync(ChangeProgramStatusInput input, CancellationToken ct);
}

