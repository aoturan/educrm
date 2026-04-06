using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ListActive;

public interface IListActiveProgramsService
{
    Task<Result<IReadOnlyList<ListActiveProgramsResult>>> ListAsync(CancellationToken ct);
}

