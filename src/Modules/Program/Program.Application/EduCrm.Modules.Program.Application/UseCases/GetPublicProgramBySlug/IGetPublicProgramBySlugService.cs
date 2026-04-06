using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetPublicProgramBySlug;

public interface IGetPublicProgramBySlugService
{
    Task<Result<GetPublicProgramBySlugResult>> GetAsync(GetPublicProgramBySlugInput input, CancellationToken ct);
}

