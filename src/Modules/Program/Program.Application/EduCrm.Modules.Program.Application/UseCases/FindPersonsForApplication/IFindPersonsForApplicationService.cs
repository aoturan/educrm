using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.FindPersonsForApplication;

public interface IFindPersonsForApplicationService
{
    Task<Result<FindPersonsForApplicationResult>> ResolveAsync(FindPersonsForApplicationInput input, CancellationToken ct);
}
