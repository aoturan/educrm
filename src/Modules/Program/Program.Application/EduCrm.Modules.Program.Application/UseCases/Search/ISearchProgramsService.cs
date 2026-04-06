using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.Search;

public interface ISearchProgramsService
{
    Task<Result<IReadOnlyList<SearchProgramsResult>>> SearchAsync(string nameQuery, CancellationToken ct);
}

