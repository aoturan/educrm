using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.List;

public interface IListProgramsService
{
    Task<Result<ProgramListPagedResult>> ListAsync(ListProgramsInput input, CancellationToken ct);
}
