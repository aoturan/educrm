using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetById;

public interface IGetProgramByIdService
{
    Task<Result<GetProgramByIdResult>> GetAsync(Guid programId, CancellationToken ct);
}

