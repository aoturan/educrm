using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.UpdateProgram;

public interface IUpdateProgramService
{
    Task<Result<UpdateProgramResult>> UpdateAsync(UpdateProgramInput input, CancellationToken ct);
}

